using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.mvc.Models;
using Templify.Domain.Entities;
using System.Security.Claims;
using Templify.Application.Features.Users.Queries;
using MediatR;
using Templify.Application.Features.Users.Commands;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Interfaces.Services;

namespace Templify.mvc.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        private readonly ILogger<SettingsController> _logger;
        private readonly IAuthorService _authorService;

        public SettingsController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IMediator mediator, 
            ILogger<SettingsController> logger,
            IAuthorService authorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _logger = logger;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTab"] = "settings";
            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appUser = await _mediator.Send(new GetAppUserByIdentityIdQuery { IdentityId = identityId });
            
            if (appUser == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var settingsModel = new SettingsViewModel
            {
                Username = appUser.Username,
                Email = appUser.Identity.Email ?? string.Empty,
                Description = appUser.Description,
                Avatar = appUser.Avatar,
                CurrentAvatarUrl = appUser.Avatar
            };

            return View(settingsModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appUser = await _mediator.Send(new GetAppUserByIdentityIdQuery { IdentityId = identityId });

            if (appUser == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            try
            {
                string? avatarPath = null;

                // Обрабатываем загрузку аватара
                if (model.AvatarFile != null && model.AvatarFile.Length > 0)
                {
                    try
                    {
                        // Проверяем размер файла (5MB)
                        if (model.AvatarFile.Length > 5 * 1024 * 1024)
                        {
                            TempData["Error"] = "File size must be less than 5MB";
                            return View("Index", model);
                        }

                        // Проверяем тип файла
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            TempData["Error"] = "Only JPG, PNG and GIF files are allowed";
                            return View("Index", model);
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "avatars");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileName = $"{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AvatarFile.CopyToAsync(stream);
                        }

                        avatarPath = $"/upload/avatars/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Error uploading avatar: {ex.Message}";
                        return View("Index", model);
                    }
                }

                var command = new UpdateAppUserProfileCommand
                {
                    IdentityId = identityId,
                    Username = model.Username,
                    Description = model.Description,
                    Avatar = avatarPath
                };

                var result = await _mediator.Send(command);

                if (result)
                {
                    // Sync author data with updated user data
                    await _authorService.SyncAuthorWithUserAsync(identityId);
                    
                    TempData["Success"] = "Profile updated successfully!";
                }
                else
                {
                    TempData["Error"] = "An error occurred while updating your profile";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating your profile";
                return View("Index", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check your input";
                return RedirectToAction("Index");
            }

            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(identityId);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            try
            {
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                
                if (result.Succeeded)
                {
                    // Сохраняем новый пароль в открытом виде
                    user.PlainPassword = model.NewPassword;
                    await _userManager.UpdateAsync(user);
                    TempData["Success"] = "Password changed successfully!";
                }
                else
                {
                    TempData["Error"] = "Current password is incorrect";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while changing your password";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(identityId))
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            try
            {
                // Используем команду для удаления пользователя
                var command = new Application.Features.Auth.Commands.DeleteUserCommand { UserId = identityId };
                var result = await _mediator.Send(command);

                if (result)
                {
                    // Выходим из системы после удаления аккаунта
                    await _signInManager.SignOutAsync();
                    TempData["Success"] = "Your account has been successfully deleted.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Failed to delete account. Please try again.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user account: {UserId}", identityId);
                TempData["Error"] = "An error occurred while deleting your account";
                return RedirectToAction("Index");
            }
        }
    }
}




