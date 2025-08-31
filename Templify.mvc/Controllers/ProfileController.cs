using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.mvc.Models;
using Templify.Domain.Entities;
using System.Security.Claims;
using Templify.Application.Features.Users.Queries;
using MediatR;

namespace Templify.mvc.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(UserManager<ApplicationUser> userManager, IMediator mediator, ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTab"] = "profile";
            var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appUser = await _mediator.Send(new GetAppUserByIdentityIdQuery { IdentityId = identityId });
            if (appUser == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            
            // Получаем полный объект ApplicationUser для правильного получения ролей
            var applicationUser = await _userManager.FindByIdAsync(identityId);
            var roles = applicationUser != null ? await _userManager.GetRolesAsync(applicationUser) : new List<string>();
            
            // Временное логирование для отладки
            _logger.LogInformation($"User ID: {identityId}");
            _logger.LogInformation($"ApplicationUser found: {applicationUser != null}");
            _logger.LogInformation($"Roles: {string.Join(", ", roles)}");
            
            var profileModel = new ProfileModel
            {
                Id = appUser.Id.ToString(),
                Email = appUser.Identity.Email ?? string.Empty,
                Username = appUser.Username,
                Avatar = !string.IsNullOrEmpty(appUser.Avatar) ? appUser.Avatar : null,
                CurrentAvatarUrl = !string.IsNullOrEmpty(appUser.Avatar) ? appUser.Avatar : null,
                Description = appUser.Description,
                CreatedAt = appUser.CreatedAt,
                LastLoginAt = appUser.Identity.LastLoginAt,
                Roles = roles.ToList()
            };
            return View(profileModel);
        }
    }
}

