using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FarmSync.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? Context.User?.FindFirst("userId")?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            // Add user to their personal group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            _logger.LogInformation($"User {userId} connected to NotificationHub with connection {Context.ConnectionId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? Context.User?.FindFirst("userId")?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            _logger.LogInformation($"User {userId} disconnected from NotificationHub");
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Client can call this to confirm they're listening
    public async Task Subscribe()
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? Context.User?.FindFirst("userId")?.Value;
        _logger.LogInformation($"User {userId} subscribed to notifications");
        await Task.CompletedTask;
    }
}
