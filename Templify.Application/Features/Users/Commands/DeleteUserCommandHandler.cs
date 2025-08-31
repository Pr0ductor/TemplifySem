using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenericRepository<AppUser> _userRepository;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, IGenericRepository<AppUser> userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userRepository.Entities.Include(u => u.Identity).FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (appUser == null) return false;
        var user = appUser.Identity;
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        await _userRepository.DeleteAsync(appUser);
        return true;
    }
}
