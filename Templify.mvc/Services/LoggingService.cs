using Microsoft.Extensions.Logging;

namespace Templify.mvc.Services
{
    public interface ILoggingService
    {
        void LogUserAction(string action, string details, string userId = null);
        void LogSecurityEvent(string eventType, string details, string userId = null, string ipAddress = null);
        void LogBusinessEvent(string eventType, string details, object data = null);
        void LogDatabaseOperation(string operation, string table, string details, TimeSpan? duration = null);
        void LogPerformanceMetric(string metricName, double value, string unit = "ms");
    }

    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogUserAction(string action, string details, string userId = null)
        {
            var user = userId ?? "Anonymous";
            _logger.LogInformation("👤 {Action}: {Details} | User: {User}", action, details, user);
        }

        public void LogSecurityEvent(string eventType, string details, string userId = null, string ipAddress = null)
        {
            var user = userId ?? "Anonymous";
            var ip = ipAddress ?? "Unknown";
            _logger.LogWarning("🔒 {EventType}: {Details} | User: {User} | IP: {IP}", eventType, details, user, ip);
        }

        public void LogBusinessEvent(string eventType, string details, object data = null)
        {
            if (data != null)
            {
                _logger.LogInformation("💼 {EventType}: {Details} | Data: {@Data}", eventType, details, data);
            }
            else
            {
                _logger.LogInformation("💼 {EventType}: {Details}", eventType, details);
            }
        }

        public void LogDatabaseOperation(string operation, string table, string details, TimeSpan? duration = null)
        {
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
        }

        public void LogPerformanceMetric(string metricName, double value, string unit = "ms")
        {
            if (value > 1000)
            {
                _logger.LogWarning("⏱️ Performance issue: {Metric} = {Value}{Unit}", metricName, value, unit);
            }
            else
            {
                _logger.LogDebug("⏱️ {Metric}: {Value}{Unit}", metricName, value, unit);
            }
        }
    }
}
