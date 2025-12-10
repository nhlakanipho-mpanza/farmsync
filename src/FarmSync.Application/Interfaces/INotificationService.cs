using FarmSync.Application.DTOs.Notifications;
using FarmSync.Domain.Entities.Notifications;

namespace FarmSync.Application.Interfaces;

public interface INotificationService
{
    Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto);
    Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, bool unreadOnly = false);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task<bool> MarkAsReadAsync(Guid notificationId);
    Task<bool> MarkAllAsReadAsync(Guid userId);
    Task<NotificationSettingDto?> GetUserSettingsAsync(Guid userId);
    Task<NotificationSettingDto> UpdateUserSettingsAsync(Guid userId, UpdateNotificationSettingDto dto);
    Task SendNotificationAsync(Guid userId, NotificationType type, string title, string message, string? actionUrl = null, string? data = null);
}
