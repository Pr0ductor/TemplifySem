using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Features.Auth.Queries;
using Templify.mvc.Models;

namespace Templify.mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["ActiveTab"] = "auth";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt failed - invalid model state from IP: {IP}", GetClientIP());
                return View("Index", model);
            }

            var query = new LoginQuery
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            var result = await _mediator.Send(query);
            
            if (result)
            {
                _logger.LogInformation("User {Email} logged in successfully from IP: {IP}", model.Email, GetClientIP());
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Failed login attempt for email: {Email} from IP: {IP}", model.Email, GetClientIP());
            ModelState.AddModelError("", "Неверный email или пароль");
            return View("Index", model);
        }

        public IActionResult Register()
        {
            ViewData["ActiveTab"] = "register";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration attempt failed - invalid model state from IP: {IP}", GetClientIP());
                return View("Register", model);
            }

            var command = new RegisterCommand
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            var result = await _mediator.Send(command);
            
            if (result)
            {
                _logger.LogInformation("New user registered: {Username} ({Email}) from IP: {IP}", model.Username, model.Email, GetClientIP());
                
                // Пользователь уже залогинен после регистрации, перенаправляем на главную
                TempData["SuccessMessage"] = $"Welcome, {model.Username}! Your account has been created successfully.";
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Failed registration attempt for email: {Email} from IP: {IP}", model.Email, GetClientIP());
            ModelState.AddModelError("", "Ошибка при регистрации. Возможно, пользователь с таким email уже существует.");
            return View("Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var currentUser = User?.Identity?.Name;
            _logger.LogInformation("User {User} logged out from IP: {IP}", currentUser, GetClientIP());
            
            // Здесь можно отправить LogoutCommand через MediatR, но для простоты используем прямой выход
            await _mediator.Send(new LogoutCommand());
            return RedirectToAction("Index", "Home");
        }

        private string GetClientIP()
        {
            var forwarded = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }

            var realIP = Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIP))
            {
                return realIP;
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}
