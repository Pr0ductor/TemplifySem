using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Repositories;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<bool> UpdateAsync(ApplicationUser user);
}

