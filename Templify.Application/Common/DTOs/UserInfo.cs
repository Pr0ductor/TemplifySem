namespace Templify.Application.Common.DTOs;

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public List<string> Roles { get; set; } = new();
}

