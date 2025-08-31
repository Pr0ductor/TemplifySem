using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Templify.Persistence.Contexts;

namespace Templify.Persistence.Repositories
{
    public class ProductPurchaseRepository : IProductPurchaseRepository
    {
        private readonly IGenericRepository<ProductPurchase> _repository;

        public ProductPurchaseRepository(IGenericRepository<ProductPurchase> repository)
        {
            _repository = repository;
        }

        public IQueryable<ProductPurchase> Entities => _repository.Entities;
        public async Task<List<ProductPurchaseDto>> GetAllAsync()
        {
            var purchases = await _repository.Entities
                .Include(p => p.AppUser)
                .Include(p => p.Product)
                .ThenInclude(p => p.AuthorEntity)
                .ToListAsync();
            
            return purchases.Select(MapToDto).ToList();
        }
        
        public async Task<ProductPurchaseDto?> GetByIdAsync(int id)
        {
            var purchase = await _repository.Entities
                .Include(p => p.AppUser)
                .Include(p => p.Product)
                .ThenInclude(p => p.AuthorEntity)
                .FirstOrDefaultAsync(p => p.Id == id);
            return purchase != null ? MapToDto(purchase) : null;
        }

        public async Task<ProductPurchase?> GetEntityByIdAsync(int id)
        {
            return await _repository.Entities
                .Include(p => p.AppUser)
                .Include(p => p.Product)
                .ThenInclude(p => p.AuthorEntity)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<ProductPurchase> AddAsync(ProductPurchase entity) => _repository.AddAsync(entity);
        public Task UpdateAsync(ProductPurchase entity) => _repository.UpdateAsync(entity);
        public Task DeleteAsync(ProductPurchase entity) => _repository.DeleteAsync(entity);
        public Task SaveChangesAsync() => _repository.SaveChangesAsync();

        private static ProductPurchaseDto MapToDto(ProductPurchase purchase)
        {
            return new ProductPurchaseDto
            {
                Id = purchase.Id,
                UserId = purchase.AppUserId,
                ProductId = purchase.ProductId,
                PurchaseDate = purchase.PurchasedAt,
                Price = purchase.Product?.Price ?? 0, // Берем цену из продукта
                Status = "completed", // В сущности нет поля Status
                UserName = purchase.AppUser?.Username ?? string.Empty,
                UserEmail = purchase.AppUser?.Identity?.Email ?? string.Empty,
                UserAvatarUrl = purchase.AppUser?.Avatar ?? string.Empty,
                ProductName = purchase.Product?.Name ?? string.Empty,
                ProductImageUrl = purchase.Product?.ImageUrl ?? string.Empty,
                ProductCategory = purchase.Product?.CategoryType.ToString() ?? string.Empty,
                AuthorName = purchase.Product?.AuthorEntity?.Name ?? purchase.Product?.Author ?? string.Empty,
                AuthorAvatarUrl = purchase.Product?.AuthorEntity?.AvatarUrl ?? string.Empty
            };
        }
    }
}
