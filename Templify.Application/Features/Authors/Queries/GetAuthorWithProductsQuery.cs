using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Authors.Queries;

public record GetAuthorWithProductsQuery : IRequest<AuthorWithProductsDto?>
{
    public int Id { get; init; }
}

public class AuthorWithProductsDto
{
    public AuthorDto Author { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = new();
}
