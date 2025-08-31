using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Templify.Domain.Common;

namespace Templify.Domain.Entities;

public class AppUser : BaseAuditableEntity
{
    [Required]
    public string IdentityId { get; set; } = string.Empty; // внешний ключ на IdentityUser

    [ForeignKey("IdentityId")]
    public ApplicationUser Identity { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<AuthorSubscription> AuthorSubscriptions { get; set; } = new List<AuthorSubscription>();
    public virtual ICollection<ProductPurchase> ProductPurchases { get; set; } = new List<ProductPurchase>();
    // Добавьте другие бизнес-поля по необходимости
}

