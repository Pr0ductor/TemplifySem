using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Infrastructure.Identity;
using Templify.Infrastructure.Services;
using Templify.Persistence.Contexts;
using Microsoft.Extensions.Configuration;

namespace Templify.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity();
            services.AddServices(configuration);
            services.AddSignalR();
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                
                // Настройки блокировки аккаунта
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        private static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            // Регистрируем сервисы
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthorSubscriptionService, AuthorSubscriptionService>();
            services.AddScoped<IProductPurchaseService, ProductPurchaseService>();
            services.AddScoped<IEmailService, EmailService>();
            
            // Новые сервисы
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
