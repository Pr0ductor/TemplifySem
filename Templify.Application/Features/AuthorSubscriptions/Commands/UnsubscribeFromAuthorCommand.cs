using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Commands;

public record UnsubscribeFromAuthorCommand : IRequest<bool>
{
    public int AuthorId { get; set; }
    public int AppUserId { get; set; }
}
