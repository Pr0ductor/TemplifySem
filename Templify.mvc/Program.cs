using Templify.Infrastructure.Extensions;
using Templify.Application.Extensions;
using Templify.Persistence.Extensions;
using Templify.mvc.Extensions;
using Microsoft.Extensions.Configuration;
using Templify.Persistence.Settings;
using Templify.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Templify.Domain.Entities;
using Templify.mvc.Services;
using Templify.mvc.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Templify.mvc.Logging;
using Templify.mvc.Services;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace Templify.mvc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Настраиваем логирование с MinIO провайдером
            // builder.Host.ConfigureCustomLogging(builder.Configuration); // Отключаем Serilog
            
            // Добавляем MinIO логгер провайдер
            var minioLoggerOptions = new MinioLoggerOptions
            {
                Endpoint = "localhost:9001",
                AccessKey = "minioadmin",
                SecretKey = "minioadmin",
                BucketName = "templify-logs",
                UseSSL = true
            };
            
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddProvider(new MinioLoggerProvider(minioLoggerOptions));
            
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer(builder.Configuration);
            builder.Services.AddPersistenceLayer(builder.Configuration);
            
            builder.Services.AddCustomLogging(builder.Configuration);
            
            // HealthChecks уже добавлены в LoggingExtensions
            
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
            
            // Настройка локализации
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "ru-RU" };
                options.SetDefaultCulture(supportedCultures[0])
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);
            });
            
            // Настройка SharedResource для общих строк
            builder.Services.Configure<LocalizationOptions>(options =>
            {
                options.ResourcesPath = "Resources";
            });
            
            // Настройка аутентификации
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "Cookies";
            })
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Auth/Index";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/Index";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });
            
            // Настройка авторизации
            builder.Services.AddAuthorization();

            var app = builder.Build();
 
            // Запись стартового лога через новый MinIO провайдер
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            logger.LogInformation("Application started - Version: {Version}, StartedAtUtc: {StartedAtUtc}", version, DateTime.UtcNow);

            // Инициализация администраторов
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                await AdminInitializer.InitializeAsync(userManager, roleManager);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/ServerError");
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Error/ServerError");
            }

            // Обработка ошибок 404, 403
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            // Добавляем middleware для локализации
            app.UseRequestLocalization();

            // Добавляем middleware для логирования
            // app.UseCustomLogging(); // Отключаем старые middleware

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<Templify.Infrastructure.Hubs.NotificationsHub>("/hubs/notifications");
 
            // Health check endpoint
            app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description,
                            duration = e.Value.Duration.ToString()
                        })
                    });
                    await context.Response.WriteAsync(result);
                }
            });

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
