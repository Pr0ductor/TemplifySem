using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.AuthorSubscriptions.Commands;

public class SubscribeToAuthorCommandHandler : IRequestHandler<SubscribeToAuthorCommand, bool>
{
    private readonly IAuthorSubscriptionService _subscriptionService;

    public SubscribeToAuthorCommandHandler(IAuthorSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(SubscribeToAuthorCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.SubscribeToAuthorAsync(request.AuthorId, request.AppUserId);
    }
}
