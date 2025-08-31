using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.AuthorSubscriptions.Queries
{
    public record GetAuthorSubscriptionByIdQuery : IRequest<AuthorSubscriptionDto?>
    {
        public int Id { get; init; }
    }
}
