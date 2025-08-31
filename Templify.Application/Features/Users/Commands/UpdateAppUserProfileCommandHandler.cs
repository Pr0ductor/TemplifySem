using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Users.Commands;

public class UpdateAppUserProfileCommandHandler : IRequestHandler<UpdateAppUserProfileCommand, bool>
{
    private readonly IGenericRepository<AppUser> _appUserRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IAuthorService _authorService;

    public UpdateAppUserProfileCommandHandler(
        IGenericRepository<AppUser> appUserRepository,
        IApplicationUserRepository applicationUserRepository,
        IAuthorService authorService)
    {
        _appUserRepository = appUserRepository;
        _applicationUserRepository = applicationUserRepository;
        _authorService = authorService;
    }

    public async Task<bool> Handle(UpdateAppUserProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var appUser = await _appUserRepository.Entities
                .Include(u => u.Identity)
                .FirstOrDefaultAsync(u => u.IdentityId == request.IdentityId);

            if (appUser == null)
                return false;

            appUser.Username = request.Username;
            appUser.Description = request.Description;
            appUser.Identity.Username = request.Username;
            appUser.Identity.Description = request.Description;

            if (!string.IsNullOrEmpty(request.Avatar))
            {
                appUser.Avatar = request.Avatar;
                appUser.Identity.Avatar = request.Avatar;
            }

            await _appUserRepository.UpdateAsync(appUser);
            await _applicationUserRepository.UpdateAsync(appUser.Identity);

            // Sync author data with updated user data
            await _authorService.SyncAuthorWithUserAsync(request.IdentityId);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
