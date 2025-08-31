using MediatR;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Products.Queries;

public class GetProductsByAuthorQueryHandler : IRequestHandler<GetProductsByAuthorQuery, IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsByAuthorQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsByAuthorQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetProductsByAuthorAsync(request.AuthorId);
    }
}
