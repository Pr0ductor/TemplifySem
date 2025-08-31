using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product entity);
    Task UpdateAsync(Product entity);
    Task DeleteAsync(Product entity);
    IQueryable<Product> Entities { get; }
    Task<IEnumerable<Product>> GetByAuthorIdAsync(int authorId);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}

