using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Users.Commands;

public record CreateUserCommand(UserEditDto User) : IRequest<int>; // возвращает Id созданного пользователя
