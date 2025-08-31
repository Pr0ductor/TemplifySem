using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Templify.Domain.Common;

namespace Templify.Domain.Entities;

public class AuthorSubscription : BaseAuditableEntity
{
    [Required]
    public int AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    public Author Author { get; set; } = null!;
    
    [Required]
    public int AppUserId { get; set; }
    
    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; } = null!;
}
