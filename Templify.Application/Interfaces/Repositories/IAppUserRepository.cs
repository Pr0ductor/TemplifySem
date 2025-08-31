using Templify.Application.Common.DTOs;

namespace Templify.Application.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<int?> GetIdByIdentityIdAsync(string identityId);
        Task<AppUserDto?> GetByIdentityIdAsync(string identityId);
    }
}




