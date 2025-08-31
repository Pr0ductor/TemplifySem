using MediatR;

namespace Templify.Application.Features.Users.Queries;

public record GetAppUserIdByIdentityIdQuery : IRequest<int?>
{
    public string IdentityId { get; set; } = string.Empty;
}

