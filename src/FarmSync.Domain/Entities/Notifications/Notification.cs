using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? Data { get; set; } // JSON for additional context
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
}

public enum NotificationType
{
    AccountCreated = 1,
    DocumentExpiringSoon = 2,
    DocumentExpired = 3,
    PurchaseOrderStatusChanged = 4,
    MaintenanceDue = 5,
    LeaveApproved = 6,
    LeaveRejected = 7,
    System = 99
}

public enum NotificationPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Urgent = 4
}
