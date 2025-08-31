using Templify.Domain.Common;
using Templify.Domain.Enums;

namespace Templify.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Author { get; set; } = string.Empty; // Keep for backward compatibility
    public string Category { get; set; } = string.Empty;
    public CategoryType CategoryType { get; set; }
    public int Downloads { get; set; }
    public string? Tags { get; set; }
    
    // Foreign key for Author
    public int AuthorId { get; set; }
    public virtual Author AuthorEntity { get; set; } = null!;

    public virtual ICollection<ProductPurchase> Purchases { get; set; } = new List<ProductPurchase>();
}

