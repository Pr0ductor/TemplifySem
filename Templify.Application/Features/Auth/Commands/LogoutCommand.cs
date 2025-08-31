using MediatR;

namespace Templify.Application.Features.Auth.Commands;

public record LogoutCommand : IRequest<bool>
{
}

