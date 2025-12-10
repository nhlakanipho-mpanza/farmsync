using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Notifications;

public class NotificationSetting : BaseEntity
{
    public Guid UserId { get; set; }
    public bool EmailEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = true;
    public string NotificationTypesJson { get; set; } = "[]"; // Array of enabled NotificationTypes
}
