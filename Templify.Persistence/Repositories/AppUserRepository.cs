using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Persistence.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly IGenericRepository<AppUser> _repository;

    public AppUserRepository(IGenericRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task<AppUser?> GetByIdentityUserIdAsync(string identityUserId)
    {
        return await _repository.Entities
            .FirstOrDefaultAsync(u => u.IdentityId == identityUserId);
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<int?> GetIdByIdentityIdAsync(string identityId)
    {
        var appUser = await _repository.Entities
            .Where(u => u.IdentityId == identityId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();
        
        return appUser;
    }

    public async Task<AppUserDto?> GetByIdentityIdAsync(string identityId)
    {
        var appUser = await _repository.Entities
            .Include(u => u.Identity)
            .Where(u => u.IdentityId == identityId)
            .Select(u => new AppUserDto
            {
                Id = u.Id,
                Username = u.Username,
                Avatar = u.Avatar,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
                IdentityId = u.IdentityId,
                Identity = new ApplicationUserDto
                {
                    Id = u.Identity.Id,
                    Email = u.Identity.Email,
                    LastLoginAt = u.Identity.LastLoginAt
                }
            })
            .FirstOrDefaultAsync();
        
        return appUser;
    }
}


