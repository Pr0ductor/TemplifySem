using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Users.Commands;

public record UpdateUserCommand(UserEditDto User) : IRequest<bool>;
