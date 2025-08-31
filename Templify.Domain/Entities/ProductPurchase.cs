using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Templify.Domain.Common;

namespace Templify.Domain.Entities;

public class ProductPurchase : BaseAuditableEntity
{
    [Required]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    [Required]
    public int AppUserId { get; set; }
    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; } = null!;

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
}



