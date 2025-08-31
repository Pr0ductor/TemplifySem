using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Templify.Persistence.Contexts;

namespace Templify.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IGenericRepository<Product> _repository;

    public ProductRepository(IGenericRepository<Product> repository)
    {
        _repository = repository;
    }

    // Delegating standard IGenericRepository methods
    public IQueryable<Product> Entities => _repository.Entities;
    public Task<List<ProductDto>> GetAllAsync() => _repository.GetAllAsync().ContinueWith(t => t.Result.Select(MapToDto).ToList());
    public Task<ProductDto?> GetByIdAsync(int id) => _repository.GetByIdAsync(id).ContinueWith(t => t.Result != null ? MapToDto(t.Result) : null);
    public Task<Product> AddAsync(Product entity) => _repository.AddAsync(entity);
    public Task UpdateAsync(Product entity) => _repository.UpdateAsync(entity);
    public Task DeleteAsync(Product entity) => _repository.DeleteAsync(entity);

    // Specific Product methods
    public async Task<IEnumerable<Product>> GetByAuthorIdAsync(int authorId)
    {
        return await _repository.Entities
            .Include(p => p.AuthorEntity)
            .Where(p => p.AuthorId == authorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _repository.Entities
            .Include(p => p.AuthorEntity)
            .Where(p => p.Name.ToLower().Contains(term) || 
                       p.Description.ToLower().Contains(term) || 
                       p.Tags.ToLower().Contains(term) || 
                       p.Category.ToLower().Contains(term))
            .OrderByDescending(p => p.Downloads)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _repository.Entities
            .Include(p => p.AuthorEntity)
            .Where(p => p.Category == category)
            .OrderByDescending(p => p.Downloads)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _repository.Entities
            .Include(p => p.AuthorEntity)
            .Where(p => p.AuthorEntity.IsActive)
            .ToListAsync();
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            CategoryType = product.CategoryType,
            Tags = product.Tags,
            ImageUrl = product.ImageUrl,
            Downloads = product.Downloads,
            AuthorId = product.AuthorId,
            Author = product.AuthorEntity?.Name ?? "Unknown Author",
            AuthorAvatarUrl = product.AuthorEntity?.AvatarUrl ?? "/src/img/person1.jpg",
        };
    }
}
