using MediatR;

namespace Templify.Application.Features.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;
