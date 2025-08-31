using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Templify.Domain.Common;
using Templify.Domain.Entities;

namespace Templify.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Author> Authors { get; set; }
        //public DbSet<AuthorSub> AuthorSubs { get; set; }
        //public DbSet<TemplateSub> TemplateSubs { get; set; }
        public DbSet<AuthorSubscription> AuthorSubscriptions { get; set; }
        public DbSet<ProductPurchase> ProductPurchases { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Глобальная конфигурация для DateTime в PostgreSQL
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("UpdatedDate")
                        .HasColumnType("timestamp with time zone");
                }
            }
            
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.AppUser)
                .WithOne(au => au.Identity)
                .HasForeignKey<AppUser>(au => au.IdentityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.ApplyConfiguration(new Configurations.ProductConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AuthorSubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProductPurchaseConfiguration());

            // Настройка ролей по умолчанию
            SeedRoles(modelBuilder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Администратор системы",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new ApplicationRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Обычный пользователь",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new ApplicationRole
                {
                    Id = "3",
                    Name = "Author",
                    NormalizedName = "AUTHOR",
                    Description = "Автор шаблонов",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        }
    }
}
