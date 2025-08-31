using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Repositories
{
    public interface IProductPurchaseRepository
    {
        Task<List<ProductPurchaseDto>> GetAllAsync();
        Task<ProductPurchaseDto?> GetByIdAsync(int id);
        Task<ProductPurchase?> GetEntityByIdAsync(int id);
        Task<ProductPurchase> AddAsync(ProductPurchase entity);
        Task UpdateAsync(ProductPurchase entity);
        Task DeleteAsync(ProductPurchase entity);
        Task SaveChangesAsync();
        IQueryable<ProductPurchase> Entities { get; }
    }
}
