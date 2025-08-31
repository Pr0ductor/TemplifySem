using MediatR;

namespace Templify.Application.Features.Users.Commands;

public record UpdateAppUserProfileCommand : IRequest<bool>
{
    public string IdentityId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Avatar { get; set; }
}

