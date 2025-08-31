using Templify.Application.Features.Auth.Queries;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password, bool rememberMe);
    Task<bool> RegisterAsync(string username, string email, string password);
    Task<(bool Success, string Email, string Password)> RegisterWithCredentialsAsync(string username, string email, string password);
    Task<bool> LogoutAsync();
    Task<bool> DeleteUserAsync(string userId);
}


