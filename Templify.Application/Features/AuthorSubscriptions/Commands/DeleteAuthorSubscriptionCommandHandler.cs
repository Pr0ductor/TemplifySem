using MediatR;
using Templify.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public class DeleteAuthorSubscriptionCommandHandler : IRequestHandler<DeleteAuthorSubscriptionCommand, bool>
    {
        private readonly IAuthorSubscriptionRepository _subscriptionRepository;
        private readonly ILogger<DeleteAuthorSubscriptionCommandHandler> _logger;

        public DeleteAuthorSubscriptionCommandHandler(
            IAuthorSubscriptionRepository subscriptionRepository,
            ILogger<DeleteAuthorSubscriptionCommandHandler> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAuthorSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingSubscription = await _subscriptionRepository.GetEntityByIdAsync(request.Id);
                if (existingSubscription == null)
                {
                    _logger.LogWarning("AuthorSubscription with ID {SubscriptionId} not found for deletion", request.Id);
                    return false;
                }

                await _subscriptionRepository.DeleteAsync(existingSubscription);
                await _subscriptionRepository.SaveChangesAsync();

                _logger.LogInformation("AuthorSubscription deleted successfully: {SubscriptionId}", existingSubscription.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting AuthorSubscription with ID {SubscriptionId}", request.Id);
                return false;
            }
        }
    }
}
