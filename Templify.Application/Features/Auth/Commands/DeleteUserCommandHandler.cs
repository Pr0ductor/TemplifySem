using MediatR;
using Templify.Application.Features.Auth.Commands;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Auth.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IAuthService _authService;

    public DeleteUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await _authService.DeleteUserAsync(request.UserId);
    }
}

