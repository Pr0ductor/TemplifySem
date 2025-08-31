using System.ComponentModel.DataAnnotations;

namespace Templify.Application.Common.DTOs;

public class ApplicationUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? LastLoginAt { get; set; }
}

