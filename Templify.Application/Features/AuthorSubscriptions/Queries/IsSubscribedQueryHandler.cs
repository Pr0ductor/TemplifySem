using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.AuthorSubscriptions.Queries;

public class IsSubscribedQueryHandler : IRequestHandler<IsSubscribedQuery, bool>
{
    private readonly IAuthorSubscriptionService _subscriptionService;

    public IsSubscribedQueryHandler(IAuthorSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(IsSubscribedQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.IsSubscribedAsync(request.AuthorId, request.AppUserId);
    }
}
