using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Templify.mvc.Middleware
{
    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceLoggingMiddleware> _logger;
        private readonly long _slowRequestThresholdMs;

        public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _slowRequestThresholdMs = configuration.GetValue<long>("Logging:SlowRequestThresholdMs", 1000);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMs = stopwatch.ElapsedMilliseconds;
                
                // Логируем медленные запросы
                if (elapsedMs > _slowRequestThresholdMs)
                {
                    var user = context.User?.Identity?.Name ?? "Anonymous";
                    var ip = GetClientIP(context);
                    
                    _logger.LogWarning("🐌 Slow request: {Method} {Path} | Duration: {Duration}ms | Threshold: {Threshold}ms | User: {User} | IP: {IP}", 
                        context.Request.Method, context.Request.Path, elapsedMs, _slowRequestThresholdMs, user, ip);
                }
                
                // Логируем статистику производительности
                _logger.LogDebug("⏱️ {Method} {Path} | Duration: {Duration}ms | Status: {Status}", 
                    context.Request.Method, context.Request.Path, elapsedMs, context.Response.StatusCode);
            }
        }

        private static string GetClientIP(HttpContext context)
        {
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }

            var realIP = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIP))
            {
                return realIP;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}
