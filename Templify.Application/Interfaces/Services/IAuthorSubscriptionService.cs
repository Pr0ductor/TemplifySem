namespace Templify.Application.Interfaces.Services;

public interface IAuthorSubscriptionService
{
    Task<bool> SubscribeToAuthorAsync(int authorId, int appUserId);
    Task<bool> UnsubscribeFromAuthorAsync(int authorId, int appUserId);
    Task<bool> IsSubscribedAsync(int authorId, int appUserId);
    Task<List<int>> GetSubscribedAuthorIdsAsync(int appUserId);
    Task<List<int>> GetSubscriberIdsAsync(int authorId);
    Task<int> GetSubscriberCountAsync(int authorId);
}
