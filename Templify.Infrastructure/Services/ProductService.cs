using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;

namespace Templify.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.Entities
            .Include(p => p.AuthorEntity)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {id} not found");
        }
        return MapToDto(product);
    }

    public async Task<Product?> GetProductEntityByIdAsync(int id)
    {
        var product = await _productRepository.Entities
            .Include(p => p.AuthorEntity)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.Entities
            .Include(p => p.AuthorEntity)
            .ToListAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByAuthorAsync(int authorId)
    {
        var products = await _productRepository.Entities
            .Where(p => p.AuthorId == authorId)
            .Include(p => p.AuthorEntity)
            .ToListAsync();

        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateProductAsync(Product product)
    {
        var createdProduct = await _productRepository.AddAsync(product);
        
        // Load the author information for the created product
        var productWithAuthor = await _productRepository.Entities
            .Include(p => p.AuthorEntity)
            .FirstOrDefaultAsync(p => p.Id == createdProduct.Id);
            
        return MapToDto(productWithAuthor ?? createdProduct);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        try
        {
            await _productRepository.UpdateAsync(product);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        try
        {
            var product = await _productRepository.Entities.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return false;
            }

            await _productRepository.DeleteAsync(product);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
    {
        var products = await _productRepository.SearchAsync(searchTerm);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return products.Select(MapToDto);
    }

    private static ProductDto MapToDto(Product product)
    {
        // Функция для исправления путей к изображениям
        string FixImagePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Убираем ~/ если есть
            if (path.StartsWith("~/"))
                path = path.Substring(2);

            // Если путь уже начинается с /, оставляем как есть
            if (path.StartsWith("/"))
                return path;

            // Если путь не начинается с /, добавляем его
            return "/" + path;
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = !string.IsNullOrEmpty(product.ImageUrl) ? FixImagePath(product.ImageUrl) : string.Empty,
            Price = product.Price,
            Category = product.Category,
            CategoryType = product.CategoryType,
            Downloads = product.Downloads,
            Tags = product.Tags,
            AuthorId = product.AuthorId,
            Author = product.AuthorEntity?.Name ?? "Unknown Author",
            AuthorAvatarUrl = !string.IsNullOrEmpty(product.AuthorEntity?.AvatarUrl) ? FixImagePath(product.AuthorEntity.AvatarUrl) : "/src/img/person1.jpg",
        };
    }
}

