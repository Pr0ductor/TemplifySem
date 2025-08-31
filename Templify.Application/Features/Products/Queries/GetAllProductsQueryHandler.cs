using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Products.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductService _productService;
        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProductsAsync();
            return products.ToList();
        }
    }
}

