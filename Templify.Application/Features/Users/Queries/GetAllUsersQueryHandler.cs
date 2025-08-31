using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IGenericRepository<AppUser> _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(IGenericRepository<AppUser> userRepository, UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.Entities
            .Include(u => u.Identity)
            .ToListAsync(cancellationToken);

        var result = new List<UserDto>();
        foreach (var u in users)
        {
            var roles = u.Identity != null ? (await _userManager.GetRolesAsync(u.Identity)).ToList() : new List<string>();
            result.Add(new UserDto
            {
                Id = u.Id,
                Nickname = u.Username,
                Email = u.Identity?.Email ?? string.Empty,
                ImageUrl = u.Avatar,
                PasswordHash = u.Identity?.PlainPassword ?? "-",
                Description = u.Description,
                Roles = roles,
                CreatedAt = u.CreatedAt
            });
        }
        return result;
    }
}
