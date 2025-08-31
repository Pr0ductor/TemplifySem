using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Templify.Application.Interfaces.Repositories;
using Templify.Persistence.Contexts;
using Templify.Persistence.Repositories;

namespace Templify.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(connectionString,
                   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient<IAppUserRepository, AppUserRepository>()
                .AddTransient<IAuthorRepository, AuthorRepository>()
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IApplicationUserRepository, ApplicationUserRepository>()
                .AddTransient<IProductPurchaseRepository, ProductPurchaseRepository>()
                .AddTransient<IAuthorSubscriptionRepository, AuthorSubscriptionRepository>();
        }
    }
}
