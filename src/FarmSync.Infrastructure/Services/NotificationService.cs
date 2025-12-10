using FarmSync.Application.DTOs.Notifications;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Notifications;
using FarmSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FarmSync.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly FarmSyncDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationService> _logger;
    private readonly INotificationHubService? _hubService;

    public NotificationService(
        FarmSyncDbContext context,
        IEmailService emailService,
        ILogger<NotificationService> logger,
        INotificationHubService? hubService = null)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
        _hubService = hubService;
    }

    public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Type = Enum.Parse<NotificationType>(dto.Type),
            Title = dto.Title,
            Message = dto.Message,
            ActionUrl = dto.ActionUrl,
            Data = dto.Data,
            Priority = Enum.Parse<NotificationPriority>(dto.Priority),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Notification created for user {dto.UserId}: {dto.Title}");

        return MapToDto(notification);
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId);

        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();

        return notifications.Select(MapToDto).ToList();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task<bool> MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null)
            return false;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAllAsReadAsync(Guid userId)
    {
        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<NotificationSettingDto?> GetUserSettingsAsync(Guid userId)
    {
        var settings = await _context.NotificationSettings
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (settings == null)
        {
            // Create default settings
            settings = new NotificationSetting
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EmailEnabled = true,
                PushEnabled = true,
                NotificationTypesJson = JsonSerializer.Serialize(Enum.GetValues<NotificationType>().Select(t => t.ToString()).ToList()),
                CreatedAt = DateTime.UtcNow
            };

            _context.NotificationSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return MapSettingsToDto(settings);
    }

    public async Task<NotificationSettingDto> UpdateUserSettingsAsync(Guid userId, UpdateNotificationSettingDto dto)
    {
        var settings = await _context.NotificationSettings
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (settings == null)
        {
            settings = new NotificationSetting
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.NotificationSettings.Add(settings);
        }

        settings.EmailEnabled = dto.EmailEnabled;
        settings.PushEnabled = dto.PushEnabled;
        settings.NotificationTypesJson = JsonSerializer.Serialize(dto.EnabledNotificationTypes);

        await _context.SaveChangesAsync();

        return MapSettingsToDto(settings);
    }

    public async Task SendNotificationAsync(Guid userId, NotificationType type, string title, string message, string? actionUrl = null, string? data = null)
    {
        // Create in-app notification
        var dto = new CreateNotificationDto
        {
            UserId = userId,
            Type = type.ToString(),
            Title = title,
            Message = message,
            ActionUrl = actionUrl,
            Data = data,
            Priority = GetPriorityForType(type).ToString()
        };

        var notification = await CreateNotificationAsync(dto);

        // Send real-time notification via SignalR
        if (_hubService != null)
        {
            try
            {
                await _hubService.SendNotificationToUserAsync(userId, notification);
                _logger.LogInformation($"Real-time notification sent to user {userId} via SignalR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send SignalR notification to user {userId}");
            }
        }

        // Check user settings for email
        var settings = await GetUserSettingsAsync(userId);
        if (settings == null || !settings.EmailEnabled)
            return;

        var enabledTypes = settings.EnabledNotificationTypes;
        if (!enabledTypes.Contains(type.ToString()))
            return;

        // Send email if enabled
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user?.Email != null)
            {
                await SendEmailForNotificationType(user.Email, type, title, message, data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email notification to user {userId}");
        }
    }

    private NotificationPriority GetPriorityForType(NotificationType type)
    {
        return type switch
        {
            NotificationType.DocumentExpired => NotificationPriority.Urgent,
            NotificationType.DocumentExpiringSoon => NotificationPriority.High,
            NotificationType.MaintenanceDue => NotificationPriority.High,
            NotificationType.AccountCreated => NotificationPriority.Normal,
            NotificationType.PurchaseOrderStatusChanged => NotificationPriority.Normal,
            NotificationType.LeaveApproved => NotificationPriority.Normal,
            NotificationType.LeaveRejected => NotificationPriority.High,
            _ => NotificationPriority.Normal
        };
    }

    private async Task SendEmailForNotificationType(string email, NotificationType type, string title, string message, string? data)
    {
        // Parse data if available
        var dataObj = data != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(data) : null;

        switch (type)
        {
            case NotificationType.DocumentExpiringSoon:
                if (dataObj != null && dataObj.ContainsKey("documentType") && dataObj.ContainsKey("expiryDate") && dataObj.ContainsKey("daysUntilExpiry"))
                {
                    await _emailService.SendDocumentExpiryWarningAsync(
                        email,
                        dataObj["documentType"],
                        DateTime.Parse(dataObj["expiryDate"]),
                        int.Parse(dataObj["daysUntilExpiry"])
                    );
                }
                break;

            case NotificationType.DocumentExpired:
                if (dataObj != null && dataObj.ContainsKey("documentType") && dataObj.ContainsKey("expiryDate"))
                {
                    await _emailService.SendDocumentExpiredNotificationAsync(
                        email,
                        dataObj["documentType"],
                        DateTime.Parse(dataObj["expiryDate"])
                    );
                }
                break;

            default:
                // Send generic email
                await _emailService.SendEmailAsync(email, title, $"<p>{message}</p>");
                break;
        }
    }

    private NotificationDto MapToDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Type = notification.Type.ToString(),
            Title = notification.Title,
            Message = notification.Message,
            ActionUrl = notification.ActionUrl,
            Data = notification.Data,
            IsRead = notification.IsRead,
            ReadAt = notification.ReadAt,
            Priority = notification.Priority.ToString(),
            CreatedAt = notification.CreatedAt
        };
    }

    private NotificationSettingDto MapSettingsToDto(NotificationSetting settings)
    {
        var enabledTypes = JsonSerializer.Deserialize<List<string>>(settings.NotificationTypesJson) ?? new List<string>();

        return new NotificationSettingDto
        {
            Id = settings.Id,
            UserId = settings.UserId,
            EmailEnabled = settings.EmailEnabled,
            PushEnabled = settings.PushEnabled,
            EnabledNotificationTypes = enabledTypes
        };
    }
}
