using FarmSync.Application.DTOs.Notifications;

namespace FarmSync.Application.Interfaces;

public interface INotificationHubService
{
    Task SendNotificationToUserAsync(Guid userId, NotificationDto notification);
}
