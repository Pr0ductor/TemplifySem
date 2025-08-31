using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templify.Domain.Entities;

namespace Templify.Persistence.Configurations;

public class ProductPurchaseConfiguration : IEntityTypeConfiguration<ProductPurchase>
{
    public void Configure(EntityTypeBuilder<ProductPurchase> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.AppUserId).IsRequired();
        
        // Конфигурация для правильной работы с DateTime в PostgreSQL
        builder.Property(p => p.PurchasedAt)
            .HasColumnType("timestamp with time zone");
        
        builder.HasIndex(p => new { p.ProductId, p.AppUserId }).IsUnique();
        builder.HasOne(p => p.Product)
            .WithMany(p => p.Purchases)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(p => p.AppUser)
            .WithMany(u => u.ProductPurchases)
            .HasForeignKey(p => p.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}



