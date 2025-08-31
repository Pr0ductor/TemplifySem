using Microsoft.AspNetCore.Identity;

namespace Templify.Domain.Entities;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}

