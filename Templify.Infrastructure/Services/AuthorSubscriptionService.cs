using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Application.Interfaces.Repositories;

namespace Templify.Infrastructure.Services;

public class AuthorSubscriptionService : IAuthorSubscriptionService
{
    private readonly IGenericRepository<AuthorSubscription> _subscriptionRepository;
    private readonly IGenericRepository<Author> _authorRepository;
    private readonly IGenericRepository<AppUser> _appUserRepository;
    private readonly ILogger<AuthorSubscriptionService> _logger;

    public AuthorSubscriptionService(
        IGenericRepository<AuthorSubscription> subscriptionRepository,
        IGenericRepository<Author> authorRepository,
        IGenericRepository<AppUser> appUserRepository,
        ILogger<AuthorSubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _authorRepository = authorRepository;
        _appUserRepository = appUserRepository;
        _logger = logger;
    }

    public async Task<bool> SubscribeToAuthorAsync(int authorId, int appUserId)
    {
        try
        {
            // Проверяем, существует ли уже подписка
            var existingSubscription = await _subscriptionRepository.Entities
                .FirstOrDefaultAsync(s => s.AuthorId == authorId && s.AppUserId == appUserId);

            if (existingSubscription != null)
            {
                _logger.LogWarning("Subscription already exists for author {AuthorId} and user {AppUserId}", authorId, appUserId);
                return false;
            }

            // Проверяем, существуют ли автор и пользователь
            var author = await _authorRepository.GetByIdAsync(authorId);
            var appUser = await _appUserRepository.GetByIdAsync(appUserId);

            if (author == null || appUser == null)
            {
                _logger.LogWarning("Author {AuthorId} or AppUser {AppUserId} not found", authorId, appUserId);
                return false;
            }

            var subscription = new AuthorSubscription
            {
                AuthorId = authorId,
                AppUserId = appUserId,
            };

            await _subscriptionRepository.AddAsync(subscription);

            _logger.LogInformation("User {AppUserId} subscribed to author {AuthorId}", appUserId, authorId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subscribing user {AppUserId} to author {AuthorId}", appUserId, authorId);
            return false;
        }
    }

    public async Task<bool> UnsubscribeFromAuthorAsync(int authorId, int appUserId)
    {
        try
        {
            var subscription = await _subscriptionRepository.Entities
                .FirstOrDefaultAsync(s => s.AuthorId == authorId && s.AppUserId == appUserId);

            if (subscription == null)
            {
                _logger.LogWarning("Subscription not found for author {AuthorId} and user {AppUserId}", authorId, appUserId);
                return false;
            }

            await _subscriptionRepository.DeleteAsync(subscription);

            _logger.LogInformation("User {AppUserId} unsubscribed from author {AuthorId}", appUserId, authorId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unsubscribing user {AppUserId} from author {AuthorId}", appUserId, authorId);
            return false;
        }
    }

    public async Task<bool> IsSubscribedAsync(int authorId, int appUserId)
    {
        try
        {
            return await _subscriptionRepository.Entities
                .AnyAsync(s => s.AuthorId == authorId && s.AppUserId == appUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking subscription status for author {AuthorId} and user {AppUserId}", authorId, appUserId);
            return false;
        }
    }

    public async Task<List<int>> GetSubscribedAuthorIdsAsync(int appUserId)
    {
        try
        {
            return await _subscriptionRepository.Entities
                .Where(s => s.AppUserId == appUserId)
                .Select(s => s.AuthorId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subscribed author IDs for user {AppUserId}", appUserId);
            return new List<int>();
        }
    }

    public async Task<List<int>> GetSubscriberIdsAsync(int authorId)
    {
        try
        {
            return await _subscriptionRepository.Entities
                .Where(s => s.AuthorId == authorId)
                .Select(s => s.AppUserId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subscriber IDs for author {AuthorId}", authorId);
            return new List<int>();
        }
    }

    public async Task<int> GetSubscriberCountAsync(int authorId)
    {
        try
        {
            return await _subscriptionRepository.Entities
                .CountAsync(s => s.AuthorId == authorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting subscriber count for author {AuthorId}", authorId);
            return 0;
        }
    }
}
