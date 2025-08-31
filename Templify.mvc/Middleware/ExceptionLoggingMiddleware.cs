using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Templify.mvc.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(context, ex);
                throw;
            }
        }

        private async Task LogExceptionAsync(HttpContext context, Exception exception)
        {
            var user = context.User?.Identity?.Name ?? "Anonymous";
            var ip = GetClientIP(context);
            
            // Логируем исключение с детальной информацией
            _logger.LogError(exception, "💥 Exception: {Type} | Message: {Message} | Path: {Path} | User: {User} | IP: {IP}", 
                exception.GetType().Name, exception.Message, context.Request.Path, user, ip);

            // Если это API запрос, возвращаем JSON ответ
            if (context.Request.Path.StartsWithSegments("/api") || 
                context.Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                await HandleApiExceptionAsync(context, exception);
            }
            else
            {
                // Для обычных запросов перенаправляем на страницу ошибки
                context.Response.Redirect($"/Error/ServerError?errorId={Guid.NewGuid()}");
            }
        }

        private async Task HandleApiExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Error = new
                {
                    Message = _environment.IsDevelopment() ? exception.Message : "An error occurred while processing your request.",
                    Type = exception.GetType().Name,
                    Details = _environment.IsDevelopment() ? exception.StackTrace : null,
                    Timestamp = DateTime.UtcNow,
                    RequestId = context.TraceIdentifier
                }
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
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
