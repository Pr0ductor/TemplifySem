using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.AuthorSubscriptions.Queries
{
    public class GetAuthorSubscriptionByIdQueryHandler : IRequestHandler<GetAuthorSubscriptionByIdQuery, AuthorSubscriptionDto?>
    {
        private readonly IAuthorSubscriptionRepository _subscriptionRepository;

        public GetAuthorSubscriptionByIdQueryHandler(IAuthorSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<AuthorSubscriptionDto?> Handle(GetAuthorSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _subscriptionRepository.GetByIdAsync(request.Id);
        }
    }
}
