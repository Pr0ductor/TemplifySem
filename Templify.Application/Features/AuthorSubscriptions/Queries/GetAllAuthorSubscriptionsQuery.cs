using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.AuthorSubscriptions.Queries
{
    public record GetAllAuthorSubscriptionsQuery : IRequest<List<AuthorSubscriptionDto>>;
}
