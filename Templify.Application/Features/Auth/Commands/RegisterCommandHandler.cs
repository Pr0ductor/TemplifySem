using MediatR;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (success, email, password) = await _authService.RegisterWithCredentialsAsync(request.Username, request.Email, request.Password);
        return success;
    }
}
