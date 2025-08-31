using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public class CreateAuthorSubscriptionCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int AuthorId { get; set; }
    }
}
