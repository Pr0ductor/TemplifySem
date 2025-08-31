using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserDto>>;
