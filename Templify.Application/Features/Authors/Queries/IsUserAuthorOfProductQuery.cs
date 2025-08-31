using MediatR;

namespace Templify.Application.Features.Authors.Queries;

public record IsUserAuthorOfProductQuery : IRequest<bool>
{
    public string UserId { get; init; } = string.Empty;
    public int ProductId { get; init; }
}
