using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Templify.mvc.Models;

public class SettingsViewModel
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public string? CurrentAvatarUrl { get; set; }
    
    [Display(Name = "Avatar")]
    public IFormFile? AvatarFile { get; set; }
}

public class ChangePasswordViewModel
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}



