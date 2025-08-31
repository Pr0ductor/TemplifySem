using Microsoft.AspNetCore.Identity;
using Templify.Domain.Common;

namespace Templify.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Username { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public string DisplayName => Username;
    
    // Навигационное свойство для связи с AppUser
    public AppUser? AppUser { get; set; }
    // Новое поле для хранения пароля в открытом виде
    public string? PlainPassword { get; set; }
}
