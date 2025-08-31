using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Users.Queries;

public class GetAppUserByIdentityIdQueryHandler : IRequestHandler<GetAppUserByIdentityIdQuery, AppUserDto?>
{
    private readonly IAppUserRepository _appUserRepository;

    public GetAppUserByIdentityIdQueryHandler(IAppUserRepository appUserRepository)
    {
        _appUserRepository = appUserRepository;
    }

    public async Task<AppUserDto?> Handle(GetAppUserByIdentityIdQuery request, CancellationToken cancellationToken)
    {
        return await _appUserRepository.GetByIdentityIdAsync(request.IdentityId);
    }
}

