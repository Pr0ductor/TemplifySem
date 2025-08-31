using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Interfaces.Services;
using Templify.Infrastructure.Hubs;
using Templify.Persistence.Contexts;
using System.Linq;

namespace Templify.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ApplicationDbContext _db;

    public NotificationService(IHubContext<NotificationsHub> hubContext, ApplicationDbContext db)
    {
        _hubContext = hubContext;
        _db = db;
    }

    public async Task NotifyAuthorSubscribersProductPublishedAsync(int authorId, int productId, string productName)
    {
        // Имя автора для сообщения
        var authorName = await _db.Authors
            .Where(a => a.Id == authorId)
            .Select(a => a.Name)
            .FirstOrDefaultAsync() ?? "Автор";

        var subscriberIds = await _db.AuthorSubscriptions
            .Where(s => s.AuthorId == authorId)
            .Select(s => s.AppUserId)
            .ToListAsync();

        if (subscriberIds.Count == 0)
        {
            return;
        }

        var identityIds = await _db.AppUsers
            .Where(u => subscriberIds.Contains(u.Id))
            .Select(u => u.IdentityId)
            .Where(id => id != null)
            .ToListAsync();

        foreach (var identityId in identityIds)
        {
            await _hubContext.Clients.Group($"user:{identityId}")
                .SendAsync("AuthorPublishedProduct", new
                {
                    authorId,
                    authorName,
                    productId,
                    productName
                });
        }
    }
}


