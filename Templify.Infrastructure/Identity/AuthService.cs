using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Persistence.Contexts;

namespace Templify.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext db,
        ILogger<AuthService> logger,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _db = db;
        _logger = logger;
        _emailService = emailService;

    }

    public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (result.Succeeded)
            {
                await _userManager.UpdateAsync(user);
                
                _logger.LogInformation("Успешный вход пользователя: {Email}", email);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при входе пользователя: {Email}", email);
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return false;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Username = username,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PlainPassword = password // Сохраняем реальный пароль
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return false;
            }

            // Создаём AppUser и связываем с IdentityUser
            var appUser = new AppUser
            {
                IdentityId = user.Id,
                Username = username,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _db.AppUsers.Add(appUser);

            // Добавляем роль "User" по умолчанию
            var userRole = await _roleManager.FindByNameAsync("User");
            if (userRole != null)
            {
                await _userManager.AddToRoleAsync(user, userRole.Name!);
            }

            await _db.SaveChangesAsync();

            // Сразу входим пользователя после регистрации
            await _signInManager.SignInAsync(user, isPersistent: false);

            _logger.LogInformation("Успешная регистрация пользователя: {Email}", email);

            var subject = "Регистрация прошла успешно";
            var body = $"<h1>Здравствуйте, {username}!</h1>" +
                       "<p>Вы успешно зарегистрировались на нашем сайте Templify.</p>" +
                       "<p>Ваши данные: </p>" +
                       $"<p>Login: {email}</p>" +
                       $"<p>Password: {password}</p>";

            await _emailService.SendEmailAsync(email, subject, body);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при регистрации пользователя: {Email}", email);
            return false;
        }
    }

    public async Task<(bool Success, string Email, string Password)> RegisterWithCredentialsAsync(string username, string email, string password)
    {
        var result = await RegisterAsync(username, email, password);
        return (result, email, password);
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Пользователь вышел из системы");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выходе из системы");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Попытка удаления несуществующего пользователя: {UserId}", userId);
                return false;
            }

            // Проверяем, является ли пользователь автором
            var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == userId);
            if (author != null)
            {
                // Удаляем все продукты автора
                var products = await _db.Products.Where(p => p.AuthorId == author.Id).ToListAsync();
                if (products.Any())
                {
                    _db.Products.RemoveRange(products);
                    _logger.LogInformation("Удалено {Count} продуктов автора {AuthorId}", products.Count, author.Id);
                }

                // Удаляем подписки на автора
                var authorSubscriptions = await _db.AuthorSubscriptions.Where(s => s.AuthorId == author.Id).ToListAsync();
                if (authorSubscriptions.Any())
                {
                    _db.AuthorSubscriptions.RemoveRange(authorSubscriptions);
                    _logger.LogInformation("Удалено {Count} подписок на автора {AuthorId}", authorSubscriptions.Count, author.Id);
                }

                // Удаляем покупки продуктов автора
                var productPurchases = await _db.ProductPurchases
                    .Where(p => products.Select(prod => prod.Id).Contains(p.ProductId))
                    .ToListAsync();
                if (productPurchases.Any())
                {
                    _db.ProductPurchases.RemoveRange(productPurchases);
                    _logger.LogInformation("Удалено {Count} покупок продуктов автора {AuthorId}", productPurchases.Count, author.Id);
                }

                // Удаляем самого автора
                _db.Authors.Remove(author);
                _logger.LogInformation("Удален автор {AuthorId} для пользователя {UserId}", author.Id, userId);
            }

            // Удаляем AppUser (это автоматически удалит ApplicationUser благодаря каскадному удалению)
            var appUser = await _db.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == userId);
            if (appUser != null)
            {
                _db.AppUsers.Remove(appUser);
            }

            await _db.SaveChangesAsync();

            // Удаляем ApplicationUser
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Пользователь успешно удален: {UserId}", userId);
                return true;
            }

            _logger.LogWarning("Ошибка при удалении пользователя: {UserId}, ошибки: {Errors}", 
                userId, string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении пользователя: {UserId}", userId);
            return false;
        }
    }
}
