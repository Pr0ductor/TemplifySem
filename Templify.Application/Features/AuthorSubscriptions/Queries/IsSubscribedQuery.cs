using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Queries;

public record IsSubscribedQuery : IRequest<bool>
{
    public int AuthorId { get; set; }
    public int AppUserId { get; set; }
}
