using MediatR;
using Templify.Application.Features.Auth.Queries;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Auth.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, bool>
{
    private readonly IAuthService _authService;

    public LoginQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Email, request.Password, request.RememberMe);
    }
}
