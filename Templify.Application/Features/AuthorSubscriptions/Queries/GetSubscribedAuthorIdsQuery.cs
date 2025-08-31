using MediatR;

namespace Templify.Application.Features.AuthorSubscriptions.Queries;

public record GetSubscribedAuthorIdsQuery : IRequest<List<int>>
{
    public int AppUserId { get; set; }
}
