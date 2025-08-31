using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Templify.mvc.Models;

public class ProfileModel
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public string? CurrentAvatarUrl { get; set; }
    public IFormFile? AvatarFile { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<string> Roles { get; set; } = new();
    
    public string DisplayName => Username;
}

