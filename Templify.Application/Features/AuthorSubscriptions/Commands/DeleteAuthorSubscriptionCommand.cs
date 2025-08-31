using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public record DeleteAuthorSubscriptionCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}
