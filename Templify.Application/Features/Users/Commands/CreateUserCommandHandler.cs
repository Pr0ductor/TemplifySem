using MediatR;
using Microsoft.AspNetCore.Identity;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGenericRepository<AppUser> _userRepository;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IGenericRepository<AppUser> userRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.User;
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            Username = dto.Username,
            Description = dto.Description,
            Avatar = dto.Avatar,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            PlainPassword = dto.Password
        };
        var result = await _userManager.CreateAsync(user, dto.Password!);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        // Добавляем роли
        if (dto.Roles != null && dto.Roles.Any())
        {
            foreach (var role in dto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new ApplicationRole { Name = role });
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        // Создаём AppUser
        var appUser = new AppUser
        {
            IdentityId = user.Id,
            Username = dto.Username,
            Avatar = dto.Avatar,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        await _userRepository.AddAsync(appUser);
        return appUser.Id;
    }
}
