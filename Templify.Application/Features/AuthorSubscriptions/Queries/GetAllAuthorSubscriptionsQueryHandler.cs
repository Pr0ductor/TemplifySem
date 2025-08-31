using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.AuthorSubscriptions.Queries
{
    public class GetAllAuthorSubscriptionsQueryHandler : IRequestHandler<GetAllAuthorSubscriptionsQuery, List<AuthorSubscriptionDto>>
    {
        private readonly IAuthorSubscriptionRepository _subscriptionRepository;

        public GetAllAuthorSubscriptionsQueryHandler(IAuthorSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<AuthorSubscriptionDto>> Handle(GetAllAuthorSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            return await _subscriptionRepository.GetAllAsync();
        }
    }
}
