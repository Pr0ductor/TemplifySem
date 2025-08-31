using MediatR;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Auth.Commands;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogoutAsync();
    }
}

