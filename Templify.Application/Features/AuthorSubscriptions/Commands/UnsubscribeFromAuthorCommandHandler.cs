using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.AuthorSubscriptions.Commands;

public class UnsubscribeFromAuthorCommandHandler : IRequestHandler<UnsubscribeFromAuthorCommand, bool>
{
    private readonly IAuthorSubscriptionService _subscriptionService;

    public UnsubscribeFromAuthorCommandHandler(IAuthorSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(UnsubscribeFromAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.UnsubscribeFromAuthorAsync(request.AuthorId, request.AppUserId);
    }
}
