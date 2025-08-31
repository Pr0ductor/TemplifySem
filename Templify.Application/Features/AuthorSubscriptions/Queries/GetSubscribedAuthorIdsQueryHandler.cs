using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.AuthorSubscriptions.Queries;

public class GetSubscribedAuthorIdsQueryHandler : IRequestHandler<GetSubscribedAuthorIdsQuery, List<int>>
{
    private readonly IAuthorSubscriptionService _subscriptionService;

    public GetSubscribedAuthorIdsQueryHandler(IAuthorSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<List<int>> Handle(GetSubscribedAuthorIdsQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetSubscribedAuthorIdsAsync(request.AppUserId);
    }
}
