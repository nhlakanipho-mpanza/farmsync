using FarmSync.API.Hubs;
using FarmSync.Application.DTOs.Notifications;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FarmSync.API.Services;

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationToUserAsync(Guid userId, NotificationDto notification)
    {
        await _hubContext.Clients.Group($"user_{userId}")
            .SendAsync("ReceiveNotification", notification);
    }
}
