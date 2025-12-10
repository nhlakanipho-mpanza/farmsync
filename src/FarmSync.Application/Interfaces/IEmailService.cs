namespace FarmSync.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendWelcomeEmailAsync(string to, string userName, string temporaryPassword);
    Task SendDocumentExpiryWarningAsync(string to, string documentType, DateTime expiryDate, int daysUntilExpiry);
    Task SendDocumentExpiredNotificationAsync(string to, string documentType, DateTime expiryDate);
    Task SendPurchaseOrderStatusChangeAsync(string to, string poNumber, string oldStatus, string newStatus);
    Task SendMaintenanceDueNotificationAsync(string to, string vehicleRegistration, DateTime dueDate);
    Task SendLeaveApprovalNotificationAsync(string to, string leaveType, bool approved, string? reason = null);
}
