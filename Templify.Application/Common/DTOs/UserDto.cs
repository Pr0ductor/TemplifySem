namespace Templify.Application.Common.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? PasswordHash { get; set; }
    public string? Description { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
