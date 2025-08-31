using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);
    Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<ProductDto>> GetProductsByAuthorAsync(int authorId);
    Task<Product?> GetProductEntityByIdAsync(int id);
}

