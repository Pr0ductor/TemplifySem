using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Templify.Infrastructure.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var identityUserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(identityUserId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{identityUserId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var identityUserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(identityUserId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{identityUserId}");
        }
        await base.OnDisconnectedAsync(exception);
    }
}



