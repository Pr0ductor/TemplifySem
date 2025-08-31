using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGenericRepository<AppUser> _userRepository;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IGenericRepository<AppUser> userRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.User;
        var appUser = await _userRepository.Entities.Include(u => u.Identity).FirstOrDefaultAsync(u => u.Id == dto.Id, cancellationToken);
        if (appUser == null) return false;
        var user = appUser.Identity;
        if (user == null) return false;

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.UserName = dto.Email;
        user.Description = dto.Description;
        user.Avatar = dto.Avatar;
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PlainPassword = dto.Password;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, dto.Password);
        }
        // Обновляем роли
        var currentRoles = await _userManager.GetRolesAsync(user);
        var toRemove = currentRoles.Except(dto.Roles ?? new List<string>()).ToList();
        var toAdd = (dto.Roles ?? new List<string>()).Except(currentRoles).ToList();
        if (toRemove.Any())
            await _userManager.RemoveFromRolesAsync(user, toRemove);
        foreach (var role in toAdd)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            await _userManager.AddToRoleAsync(user, role);
        }
        await _userManager.UpdateAsync(user);
        // AppUser
        appUser.Username = dto.Username;
        appUser.Avatar = dto.Avatar;
        appUser.Description = dto.Description;
        await _userRepository.UpdateAsync(appUser);
        return true;
    }
}
