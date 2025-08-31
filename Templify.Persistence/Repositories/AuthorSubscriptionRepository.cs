using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Templify.Persistence.Contexts;

namespace Templify.Persistence.Repositories
{
    public class AuthorSubscriptionRepository : IAuthorSubscriptionRepository
    {
        private readonly IGenericRepository<AuthorSubscription> _repository;

        public AuthorSubscriptionRepository(IGenericRepository<AuthorSubscription> repository)
        {
            _repository = repository;
        }

        public IQueryable<AuthorSubscription> Entities => _repository.Entities;
        public async Task<List<AuthorSubscriptionDto>> GetAllAsync()
        {
            var subscriptions = await _repository.Entities
                .Include(s => s.AppUser)
                .ThenInclude(u => u.Identity)
                .Include(s => s.Author)
                .ToListAsync();
            return subscriptions.Select(MapToDto).ToList();
        }
        
        public async Task<AuthorSubscriptionDto?> GetByIdAsync(int id)
        {
            var subscription = await _repository.Entities
                .Include(s => s.AppUser)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(s => s.Id == id);
            return subscription != null ? MapToDto(subscription) : null;
        }

        public async Task<AuthorSubscription?> GetEntityByIdAsync(int id)
        {
            return await _repository.Entities
                .Include(s => s.AppUser)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<AuthorSubscription> AddAsync(AuthorSubscription entity) => _repository.AddAsync(entity);
        public Task UpdateAsync(AuthorSubscription entity) => _repository.UpdateAsync(entity);
        public Task DeleteAsync(AuthorSubscription entity) => _repository.DeleteAsync(entity);
        public Task SaveChangesAsync() => _repository.SaveChangesAsync();

        private static AuthorSubscriptionDto MapToDto(AuthorSubscription subscription)
        {
            return new AuthorSubscriptionDto
            {
                Id = subscription.Id,
                UserId = subscription.AppUserId,
                AuthorId = subscription.AuthorId,
                SubscriptionDate = subscription.CreatedDate ?? DateTime.UtcNow, // Используем CreatedDate из BaseAuditableEntity
                IsActive = true, // В сущности нет поля IsActive, считаем все активными
                UserName = subscription.AppUser?.Username ?? string.Empty,
                UserEmail = subscription.AppUser?.Identity?.Email ?? string.Empty,
                UserAvatarUrl = subscription.AppUser?.Avatar ?? string.Empty,
                AuthorName = subscription.Author?.Name ?? string.Empty,
                AuthorEmail = subscription.Author?.Email ?? string.Empty,
                AuthorAvatarUrl = subscription.Author?.AvatarUrl ?? string.Empty,
                AuthorSpecialization = subscription.Author?.Specialization ?? string.Empty
            };
        }
    }
}
