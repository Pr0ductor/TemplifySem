namespace Templify.Application.Common.DTOs;

public class UserEditDto
{
    public int? Id { get; set; } // null для создания, иначе - редактирование
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public List<string> Roles { get; set; } = new();
}
