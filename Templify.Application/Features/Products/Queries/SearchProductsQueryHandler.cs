using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Enums;
using System.Linq;

namespace Templify.Application.Features.Products.Queries
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, List<ProductDto>>
    {
        private readonly IProductService _productService;
        public SearchProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var allProducts = await _productService.GetAllProductsAsync();
            
            var filteredProducts = allProducts.AsQueryable();
            
            // Фильтр по поисковому термину
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            // Фильтр по категории
            if (request.CategoryType.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryType == request.CategoryType.Value);
            }
            
            return filteredProducts.ToList();
        }
    }
}
