using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Products.Queries;

public record GetProductsByAuthorQuery : IRequest<IEnumerable<ProductDto>>
{
    public int AuthorId { get; init; }
}
