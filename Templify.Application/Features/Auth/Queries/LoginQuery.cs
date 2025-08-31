using MediatR;

namespace Templify.Application.Features.Auth.Queries;

public record LoginQuery : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}
