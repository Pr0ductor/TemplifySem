using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Users.Queries;

public record GetAppUserByIdentityIdQuery : IRequest<AppUserDto?>
{
    public string IdentityId { get; set; } = string.Empty;
}

