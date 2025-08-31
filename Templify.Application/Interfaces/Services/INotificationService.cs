namespace Templify.Application.Interfaces.Services;

public interface INotificationService
{
    Task NotifyAuthorSubscribersProductPublishedAsync(int authorId, int productId, string productName);
}



