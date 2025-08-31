using Templify.Domain.Common;

namespace Templify.Domain.Entities;

public class Author : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string SocialLinks { get; set; } = string.Empty; // JSON string for social media links
    public int TotalProducts { get; set; }
    public int TotalDownloads { get; set; }
    public string Specialization { get; set; } = string.Empty;
    
    // New fields for user integration
    public string? UserId { get; set; } // FK to ApplicationUser (nullable for seed authors)
    public bool IsSeedAuthor { get; set; } = false;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<AuthorSubscription> Subscriptions { get; set; } = new List<AuthorSubscription>();
    public virtual ApplicationUser? User { get; set; } // Navigation to ApplicationUser
}
