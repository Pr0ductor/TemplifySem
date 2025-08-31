using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using System.Text;

namespace Templify.mvc.Logging
{
    public class MinioLoggerProvider : ILoggerProvider
    {
        private readonly MinioLoggerOptions _options;
        private readonly IMinioClient _minioClient;

        public MinioLoggerProvider(MinioLoggerOptions options)
        {
            _options = options;
            
            _minioClient = new MinioClient()
                .WithEndpoint(_options.Endpoint)
                .WithCredentials(_options.AccessKey, _options.SecretKey)
                .WithSSL(_options.UseSSL)
                .Build();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MinioLogger(_minioClient, _options, categoryName);
        }

        public void Dispose()
        {
            _minioClient?.Dispose();
        }
    }

    public class MinioLoggerOptions
    {
        public string Endpoint { get; set; } = "192.168.3.12:9000";
        public string AccessKey { get; set; } = "minioadmin";
        public string SecretKey { get; set; } = "minioadmin";
        public string BucketName { get; set; } = "templify-logs";
        public bool UseSSL { get; set; } = false;
    }

    public class MinioLogger : ILogger
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioLoggerOptions _options;
        private readonly string _categoryName;
        private readonly object _lock = new object();

        public MinioLogger(IMinioClient minioClient, MinioLoggerOptions options, string categoryName)
        {
            _minioClient = minioClient;
            _options = options;
            _categoryName = categoryName;
            
            // Ensure bucket exists
            EnsureBucketExistsAsync().Wait();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var timestamp = DateTime.UtcNow;
            
            // Формат: [2025-08-29 20:03:20] [INFO] [Category] Message
            var logLine = $"[{timestamp:yyyy-MM-dd HH:mm:ss}] [{logLevel.ToString().ToUpper()}] [{_categoryName}] {message}";
            
            if (exception != null)
            {
                logLine += $"\nException: {exception}";
            }
            
            // Добавляем перенос строки
            logLine += "\n";
            
            // Путь к локальной папке logs
            var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            var logFileName = $"templify-{DateTime.UtcNow:yyyyMMdd}.txt";
            var logFilePath = Path.Combine(logsDirectory, logFileName);

            try
            {
                // Создаем папку logs если её нет
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }

                // Добавляем новую строку в конец файла
                File.AppendAllText(logFilePath, logLine);
                
                                 // Также загружаем в MinIO для дублирования
                 try
                 {
                     // Загружаем в MinIO весь файл лога, а не только последнюю строку
                     using var fileStream = File.OpenRead(logFilePath);
                     var putObjectArgs = new PutObjectArgs()
                         .WithBucket(_options.BucketName)
                         .WithObject($"logs/{logFileName}")
                         .WithStreamData(fileStream)
                         .WithObjectSize(fileStream.Length)
                         .WithContentType("text/plain");

                     _minioClient.PutObjectAsync(putObjectArgs).Wait();
                 }
                 catch (Exception minioEx)
                 {
                     // Если MinIO недоступен, логируем только локально
                     // Console.WriteLine($"[ERROR] Failed to upload to MinIO: {minioEx.Message}");
                 }
            }
            catch (Exception ex)
            {
                // Fallback to console if local write fails
                Console.WriteLine($"Failed to write log locally: {ex.Message}");
            }
        }

        private async Task EnsureBucketExistsAsync()
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs()
                    .WithBucket(_options.BucketName);

                var found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(_options.BucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs);
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Failed to ensure bucket exists: {ex.Message}");
            }
        }
    }
}
