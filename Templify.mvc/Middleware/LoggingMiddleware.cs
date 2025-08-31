using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Templify.mvc.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;
            
            // Логируем начало запроса
            var user = context.User?.Identity?.Name ?? "Anonymous";
            var ip = GetClientIP(context);
            
            _logger.LogInformation("🌐 {Method} {Path} | User: {User} | IP: {IP}", 
                context.Request.Method, context.Request.Path, user, ip);

            try
            {
                await _next(context);
                stopwatch.Stop();
                
                // Логируем завершение запроса
                var status = context.Response.StatusCode;
                var duration = stopwatch.ElapsedMilliseconds;
                
                if (status >= 400)
                {
                    _logger.LogWarning("❌ {Method} {Path} | Status: {Status} | Duration: {Duration}ms", 
                        context.Request.Method, context.Request.Path, status, duration);
                }
                else
                {
                    _logger.LogInformation("✅ {Method} {Path} | Status: {Status} | Duration: {Duration}ms", 
                        context.Request.Method, context.Request.Path, status, duration);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "💥 {Method} {Path} | Error: {Error} | Duration: {Duration}ms", 
                    context.Request.Method, context.Request.Path, ex.Message, stopwatch.ElapsedMilliseconds);
                throw;
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
