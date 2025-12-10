namespace FarmSync.Application.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? Data { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateNotificationDto
{
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? Data { get; set; }
    public string Priority { get; set; } = "Normal";
}

public class NotificationSettingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool EmailEnabled { get; set; }
    public bool PushEnabled { get; set; }
    public List<string> EnabledNotificationTypes { get; set; } = new();
}

public class UpdateNotificationSettingDto
{
    public bool EmailEnabled { get; set; }
    public bool PushEnabled { get; set; }
    public List<string> EnabledNotificationTypes { get; set; } = new();
}

public class UnreadCountDto
{
    public int UnreadCount { get; set; }
}
