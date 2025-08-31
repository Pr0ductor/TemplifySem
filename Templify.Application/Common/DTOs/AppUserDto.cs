using System.ComponentModel.DataAnnotations;

namespace Templify.Application.Common.DTOs;

public class AppUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string IdentityId { get; set; } = string.Empty;
    public ApplicationUserDto? Identity { get; set; }
}

