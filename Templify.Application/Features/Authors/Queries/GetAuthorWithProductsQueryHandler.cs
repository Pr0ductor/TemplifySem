using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Authors.Queries;

public class GetAuthorWithProductsQueryHandler : IRequestHandler<GetAuthorWithProductsQuery, AuthorWithProductsDto?>
{
    private readonly IAuthorService _authorService;
    private readonly IProductService _productService;

    public GetAuthorWithProductsQueryHandler(IAuthorService authorService, IProductService productService)
    {
        _authorService = authorService;
        _productService = productService;
    }

    public async Task<AuthorWithProductsDto?> Handle(GetAuthorWithProductsQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorService.GetAuthorByIdAsync(request.Id);
        if (author == null)
        {
            return null;
        }

        var products = await _productService.GetProductsByAuthorAsync(request.Id);
        
        return new AuthorWithProductsDto
        {
            Author = author,
            Products = products.ToList()
        };
    }
}
