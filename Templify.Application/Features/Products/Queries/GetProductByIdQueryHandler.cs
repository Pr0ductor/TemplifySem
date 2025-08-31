using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductService _productService;
        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByIdAsync(request.Id);
        }
    }
}

