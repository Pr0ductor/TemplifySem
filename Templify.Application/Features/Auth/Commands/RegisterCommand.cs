using MediatR;

namespace Templify.Application.Features.Auth.Commands;

public record RegisterCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

}


