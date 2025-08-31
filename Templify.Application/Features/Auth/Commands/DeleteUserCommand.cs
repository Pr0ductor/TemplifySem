using MediatR;

namespace Templify.Application.Features.Auth.Commands;

public record DeleteUserCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
}

