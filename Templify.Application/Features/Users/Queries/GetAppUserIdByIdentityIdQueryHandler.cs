using MediatR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Repositories;

namespace Templify.Application.Features.Users.Queries;

public class GetAppUserIdByIdentityIdQueryHandler : IRequestHandler<GetAppUserIdByIdentityIdQuery, int?>
{
    private readonly IAppUserRepository _appUserRepository;

    public GetAppUserIdByIdentityIdQueryHandler(IAppUserRepository appUserRepository)
    {
        _appUserRepository = appUserRepository;
    }

    public async Task<int?> Handle(GetAppUserIdByIdentityIdQuery request, CancellationToken cancellationToken)
    {
        return await _appUserRepository.GetIdByIdentityIdAsync(request.IdentityId);
    }
}

