using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Commands
{
    public class UpdateAuthorSubscriptionCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuthorId { get; set; }
    }
}
