using MediatR;
using Templify.Domain.Entities;
using Templify.Application.Interfaces.Repositories;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public class CreateAuthorSubscriptionCommandHandler : IRequestHandler<CreateAuthorSubscriptionCommand, int>
    {
        private readonly IGenericRepository<AuthorSubscription> _repository;

        public CreateAuthorSubscriptionCommandHandler(IGenericRepository<AuthorSubscription> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateAuthorSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = new AuthorSubscription
            {
                AppUserId = request.UserId,
                AuthorId = request.AuthorId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(subscription);
            return subscription.Id;
        }
    }
}
