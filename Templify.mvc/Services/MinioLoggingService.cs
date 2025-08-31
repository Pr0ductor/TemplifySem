using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Templify.mvc.Settings;
using System.Text.Json;

namespace Templify.mvc.Services
{
    public interface IMinioLoggingService
    {
        void LogUserAction(string action, string details, string userId = null);
        void LogSecurityEvent(string eventType, string details, string userId = null, string ipAddress = null);
        void LogBusinessEvent(string eventType, string details, object data = null);
        void LogDatabaseOperation(string operation, string table, string details, TimeSpan? duration = null);
        void LogPerformanceMetric(string metricName, double value, string unit = "ms");
        Task<bool> SyncLogsToMinioAsync();
        Task<bool> ArchiveLogsAsync();
    }

    public class MinioLoggingService : IMinioLoggingService, ILoggingService, IDisposable
    {
        private readonly ILogger<MinioLoggingService> _logger;
        private readonly IMinioService _minioService;
        private readonly MinioSettings _minioSettings;
        private readonly string _logDirectory;
        private readonly Queue<LogEntry> _logQueue;
        private readonly object _lockObject = new object();
        private readonly Timer _syncTimer;
        private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(1);

        public MinioLoggingService(
            ILogger<MinioLoggingService> logger,
            IMinioService minioService,
            IOptions<MinioSettings> minioSettings,
            IConfiguration configuration)
        {
            _logger = logger;
            _minioService = minioService;
            _minioSettings = minioSettings.Value;
            _logDirectory = configuration.GetValue<string>("Logging:FilePath", "logs");
            _logQueue = new Queue<LogEntry>();
            
            // Запускаем таймер для автоматической синхронизации каждую минуту
            _syncTimer = new Timer(_ =>
            {
                try
                {
                    _logger.LogDebug("[MinioTimer] Timer triggered - performing automatic log sync. Queue size: {Count}", _logQueue.Count);
                    Task.Run(async () =>
                    {
                        try
                        {
                            await SyncLogsToMinioAsync();
                            _logger.LogDebug("[MinioTimer] Automatic log sync finished");
                            await ArchiveLogsAsync();
                            _logger.LogDebug("[MinioTimer] ArchiveLogsAsync finished");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "[MinioTimer] Error in automatic log sync or archive");
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[MinioTimer] Error scheduling automatic log sync");
                }
            }, null, _syncInterval, _syncInterval);
            
            _logger.LogInformation("MinioLoggingService initialized - logs will sync every 5 actions or every minute automatically");
        }

        public void LogUserAction(string action, string details, string userId = null)
        {
            var user = userId ?? "Anonymous";
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = "Information",
                Category = "UserAction",
                Action = action,
                Details = details,
                UserId = user,
                IpAddress = null
            };

            _logger.LogInformation("👤 {Action}: {Details} | User: {User}", action, details, user);
            EnqueueLog(logEntry);
        }

        public void LogSecurityEvent(string eventType, string details, string userId = null, string ipAddress = null)
        {
            var user = userId ?? "Anonymous";
            var ip = ipAddress ?? "Unknown";
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = "Warning",
                Category = "SecurityEvent",
                Action = eventType,
                Details = details,
                UserId = user,
                IpAddress = ip
            };

            _logger.LogWarning("🔒 {EventType}: {Details} | User: {User} | IP: {IP}", eventType, details, user, ip);
            EnqueueLog(logEntry);
        }

        public void LogBusinessEvent(string eventType, string details, object data = null)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = "Information",
                Category = "BusinessEvent",
                Action = eventType,
                Details = details,
                UserId = null,
                IpAddress = null,
                Data = data
            };

            if (data != null)
            {
                _logger.LogInformation("💼 {EventType}: {Details} | Data: {@Data}", eventType, details, data);
            }
            else
            {
                _logger.LogInformation("💼 {EventType}: {Details}", eventType, details);
            }
            
            EnqueueLog(logEntry);
        }

        public void LogDatabaseOperation(string operation, string table, string details, TimeSpan? duration = null)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = duration?.TotalMilliseconds > 1000 ? "Warning" : "Debug",
                Category = "DatabaseOperation",
                Action = operation,
                Details = $"{table}: {details}",
                UserId = null,
                IpAddress = null,
                Duration = duration
            };

            if (duration.HasValue)
            {
                var durationMs = duration.Value.TotalMilliseconds;
                if (durationMs > 1000)
                {
                    _logger.LogWarning("🐌 Slow DB {Operation} on {Table}: {Details} | Duration: {Duration}ms", 
                        operation, table, details, durationMs);
                }
                else
                {
                    _logger.LogDebug("🗄️ DB {Operation} on {Table}: {Details} | Duration: {Duration}ms", 
                        operation, table, details, durationMs);
                }
            }
            else
            {
                _logger.LogDebug("🗄️ DB {Operation} on {Table}: {Details}", operation, table, details);
            }
            
            EnqueueLog(logEntry);
        }

        public void LogPerformanceMetric(string metricName, double value, string unit = "ms")
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = value > 1000 ? "Warning" : "Debug",
                Category = "PerformanceMetric",
                Action = metricName,
                Details = $"{value}{unit}",
                UserId = null,
                IpAddress = null,
                MetricValue = value,
                MetricUnit = unit
            };

            if (value > 1000)
            {
                _logger.LogWarning("⏱️ Performance issue: {Metric} = {Value}{Unit}", metricName, value, unit);
            }
            else
            {
                _logger.LogDebug("⏱️ {Metric}: {Value}{Unit}", metricName, value, unit);
            }
            
            EnqueueLog(logEntry);
        }

        private void EnqueueLog(LogEntry logEntry)
        {
            lock (_lockObject)
            {
                _logQueue.Enqueue(logEntry);
                
                // Синхронизируем каждые 5 действий для более частой отправки
                if (_logQueue.Count >= 5)
                {
                    _logger.LogDebug("Queue reached {Count} logs, triggering immediate sync", _logQueue.Count);
                    _ = Task.Run(async () => await SyncLogsToMinioAsync());
                }
            }
        }

        public async Task<bool> SyncLogsToMinioAsync()
        {
            try
            {
                List<LogEntry> logsToSync;
                
                lock (_lockObject)
                {
                    if (_logQueue.Count == 0)
                    {
                        _logger.LogDebug("No logs to sync - queue is empty");
                        return true;
                    }
                    
                    _logger.LogDebug("Starting sync of {Count} logs from queue", _logQueue.Count);
                    
                    logsToSync = _logQueue.ToList();
                    _logQueue.Clear();
                }

                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
                var fileName = $"templify-{timestamp}.txt";
                
                var lines = new List<string>(logsToSync.Count);
                foreach (var e in logsToSync)
                {
                    var lvl = e.Level?.Length >= 3 ? e.Level.Substring(0,3).ToUpperInvariant() : e.Level?.ToUpperInvariant();
                    var baseMsg = $"[{e.Timestamp:HH:mm:ss} {lvl}] {e.Category}/{e.Action}: {e.Details}";
                    var metaParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(e.UserId)) metaParts.Add($"User={e.UserId}");
                    if (!string.IsNullOrWhiteSpace(e.IpAddress)) metaParts.Add($"IP={e.IpAddress}");
                    if (e.Duration.HasValue) metaParts.Add($"DurationMs={e.Duration.Value.TotalMilliseconds:F0}");
                    if (e.MetricValue.HasValue) metaParts.Add($"Metric={e.MetricValue}{e.MetricUnit}");
                    var meta = metaParts.Count > 0 ? $" | {string.Join("; ", metaParts)}" : string.Empty;
                    lines.Add(baseMsg + meta);
                }
                var logContent = string.Join(Environment.NewLine, lines) + Environment.NewLine;

                var result = await _minioService.UploadLogAsync(logContent, fileName);
                
                if (result)
                {
                    _logger.LogInformation("Successfully synced {Count} logs to MinIO (txt)", logsToSync.Count);
                }
                else
                {
                    _logger.LogError("Failed to sync {Count} logs to MinIO (txt)", logsToSync.Count);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during log sync to MinIO");
                return false;
            }
        }

        public async Task<bool> ArchiveLogsAsync()
        {
            try
            {
                if (!Directory.Exists(_logDirectory))
                    return true;

                var logFiles = Directory.GetFiles(_logDirectory, "*.log")
                    .Concat(Directory.GetFiles(_logDirectory, "*.json"))
                    .Concat(Directory.GetFiles(_logDirectory, "*.txt"))
                    .Where(f => !f.Contains("current"))
                    .ToList();

                var successCount = 0;
                foreach (var filePath in logFiles)
                {
                    var fileName = Path.GetFileName(filePath);
                    var timestamp = File.GetLastWriteTime(filePath).ToString("yyyy-MM-dd-HH-mm-ss");
                    var archiveName = $"archive-{timestamp}-{fileName}";
                    
                    if (await _minioService.UploadLogFileAsync(filePath, archiveName))
                    {
                        successCount++;
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete local log file: {FilePath}", filePath);
                        }
                    }
                }

                _logger.LogInformation("Archived {SuccessCount}/{TotalCount} log files to MinIO", successCount, logFiles.Count);
                return successCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during log archiving to MinIO");
                return false;
            }
        }

        public void Dispose()
        {
            // Останавливаем таймер
            _syncTimer?.Dispose();
            
            // Принудительная синхронизация оставшихся логов при завершении
            try
            {
                if (_logQueue.Count > 0)
                {
                    _logger.LogInformation("Disposing MinioLoggingService - syncing remaining {Count} logs", _logQueue.Count);
                    _ = Task.Run(async () => await SyncLogsToMinioAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during final log sync on dispose");
            }
        }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public object? Data { get; set; }
        public TimeSpan? Duration { get; set; }
        public double? MetricValue { get; set; }
        public string? MetricUnit { get; set; }
    }
}
