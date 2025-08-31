using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Templify.mvc.Middleware;
using Templify.mvc.Services;
using Templify.mvc.Settings;

namespace Templify.mvc.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioSettings>(configuration.GetSection("MinioSettings"));
            services.AddSingleton<IMinioService, S3MinioService>();
            services.AddSingleton<MinioLoggingService>();
            services.AddSingleton<ILoggingService>(sp => sp.GetRequiredService<MinioLoggingService>());
            services.AddSingleton<IMinioLoggingService>(sp => sp.GetRequiredService<MinioLoggingService>());
            
            services.AddHealthChecks()
                .AddCheck<MinioHealthCheckService>("minio", tags: new[] { "minio", "storage" });
            
            return services;
        }

        public static IHostBuilder ConfigureCustomLogging(this IHostBuilder builder, IConfiguration configuration)
        {
            // Отключаем Serilog - теперь используем только MinioLoggerProvider
            // var logPath = configuration.GetValue<string>("Logging:FilePath", "logs");
            // 
            // var logger = new LoggerConfiguration()
            //     .MinimumLevel.Debug()
            //     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            //     .MinimumLevel.Override("System", LogEventLevel.Warning)
            //     .Enrich.FromLogContext()
            //     .Enrich.WithProperty("Application", "Templify")
            //     .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
            //     
            //     // Консоль с красивым форматированием
            //     .WriteTo.Console(
            //         theme: AnsiConsoleTheme.Code,
            //         outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}")
            //     
            //     // Отключаем файловое логирование - теперь логи идут только в MinIO
            //     // .WriteTo.File(
            //     //     path: Path.Combine(logPath, "templify-.txt"),
            //     //     rollingInterval: RollingInterval.Day,
            //     //     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            //     //     retainedFileCountLimit: 30,
            //     fileSizeLimitBytes: 5 * 1024 * 1024) // 5MB
            //     // 
            //     // // Файл ошибок
            //     // .WriteTo.File(
            //     //     path: Path.Combine(logPath, "errors-.txt"),
            //     //     restrictedToMinimumLevel: LogEventLevel.Error,
            //     //     rollingInterval: RollingInterval.Day,
            //     //     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            //     //     retainedFileCountLimit: 30,
            //     //     fileSizeLimitBytes: 5 * 1024 * 1024) // 5MB
            //     // 
            //     // // JSON файл для анализа
            //     // .WriteTo.File(
            //     //     path: Path.Combine(logPath, "templify-.txt"),
            //     //     rollingInterval: RollingInterval.Day,
            //     //     formatter: new Serilog.Formatting.Json.JsonFormatter(),
            //     //     retainedFileCountLimit: 30,
            //     //     fileSizeLimitBytes: 5 * 1024 * 1024) // 5MB
            //     
            //     .CreateLogger();
            // 
            // builder.UseSerilog(logger);
            return builder;
        }

        public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionLoggingMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<PerformanceLoggingMiddleware>();
            return app;
        }
    }
}
