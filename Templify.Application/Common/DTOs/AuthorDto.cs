namespace Templify.Application.Common.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string SocialLinks { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public int TotalDownloads { get; set; }
    public string Specialization { get; set; } = string.Empty;
    
    // User integration fields
    public string? UserId { get; set; }
    public bool IsSeedAuthor { get; set; }
    public bool IsActive { get; set; }
    public bool IsRealUser => !string.IsNullOrEmpty(UserId);
    
    // Computed properties
    public string DisplayName => Name;
    public string DisplayAvatar => AvatarUrl;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}


