using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public class UpdateAuthorSubscriptionCommandHandler : IRequestHandler<UpdateAuthorSubscriptionCommand, bool>
    {
        private readonly IAuthorSubscriptionRepository _subscriptionRepository;
        public UpdateAuthorSubscriptionCommandHandler(IAuthorSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<bool> Handle(UpdateAuthorSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetEntityByIdAsync(request.Id);
                if (subscription == null)
                    return false;
                subscription.AppUserId = request.UserId;
                subscription.AuthorId = request.AuthorId;
                subscription.CreatedDate = DateTime.UtcNow;
                await _subscriptionRepository.UpdateAsync(subscription);
                await _subscriptionRepository.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
