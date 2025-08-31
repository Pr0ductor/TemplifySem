using Microsoft.AspNetCore.Mvc;
using MediatR;
using Templify.Application.Features.Users.Queries;
using Templify.Application.Common.DTOs;
using Templify.mvc.Attributes;
using Templify.Application.Features.Users.Commands;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RequireRole("Admin")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search = "", string role = "")
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                var sorted = users.OrderBy(u => u.Id).ToList();
                
                // Передаем параметры фильтров в ViewBag для восстановления состояния
                ViewBag.SearchTerm = search;
                ViewBag.RoleFilter = role;
                
                return View(sorted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users list");
                return View(new List<UserDto>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserEditDto model, string rolesString)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            // Парсим роли из строки
            if (!string.IsNullOrEmpty(rolesString))
            {
                model.Roles = rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList();
            }
            
            try
            {
                await _mediator.Send(new CreateUserCommand(model));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            var editDto = new UserEditDto
            {
                Id = user.Id,
                Username = user.Nickname,
                Email = user.Email,
                Description = user.Description,
                Avatar = user.ImageUrl,
                Roles = user.Roles
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditDto model, string rolesString)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            // Парсим роли из строки
            if (!string.IsNullOrEmpty(rolesString))
            {
                model.Roles = rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList();
            }
            
            try
            {
                await _mediator.Send(new UpdateUserCommand(model));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return RedirectToAction("Index");
            }
        }
    }
}
