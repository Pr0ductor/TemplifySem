using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templify.Domain.Entities;

namespace Templify.Persistence.Configurations;

public class AuthorSubscriptionConfiguration : IEntityTypeConfiguration<AuthorSubscription>
{
    public void Configure(EntityTypeBuilder<AuthorSubscription> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.AuthorId)
            .IsRequired();
            
        builder.Property(s => s.AppUserId)
            .IsRequired();

        // Уникальный индекс для предотвращения дублирования подписок
        builder.HasIndex(s => new { s.AuthorId, s.AppUserId })
            .IsUnique();

        // Настройка внешних ключей
        builder.HasOne(s => s.Author)
            .WithMany(a => a.Subscriptions)
            .HasForeignKey(s => s.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.AppUser)
            .WithMany(u => u.AuthorSubscriptions)
            .HasForeignKey(s => s.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
