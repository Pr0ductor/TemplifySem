using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Repositories
{
    public interface IAuthorSubscriptionRepository
    {
        Task<List<AuthorSubscriptionDto>> GetAllAsync();
        Task<AuthorSubscriptionDto?> GetByIdAsync(int id);
        Task<AuthorSubscription?> GetEntityByIdAsync(int id);
        Task<AuthorSubscription> AddAsync(AuthorSubscription entity);
        Task UpdateAsync(AuthorSubscription entity);
        Task DeleteAsync(AuthorSubscription entity);
        Task SaveChangesAsync();
        IQueryable<AuthorSubscription> Entities { get; }
    }
}
