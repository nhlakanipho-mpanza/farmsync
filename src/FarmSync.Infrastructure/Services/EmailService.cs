using System.Net;
using System.Net.Mail;
using FarmSync.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FarmSync.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"] ?? "mail.zimeholding.co.za";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? "system@zimeholding.co.za";
            var smtpPass = _configuration["Email:SmtpPassword"] ?? "FarmSync@Zime";
            var fromName = _configuration["Email:FromName"] ?? "FarmSync System";

            using var message = new MailMessage
            {
                From = new MailAddress(smtpUser, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);
            
            // BCC testing email
            message.Bcc.Add("mpanzanhlaka@gmail.com");

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
            _logger.LogInformation($"Email sent successfully to {to}: {subject}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {to}: {subject}");
            throw;
        }
    }

    public async Task SendWelcomeEmailAsync(string to, string userName, string temporaryPassword)
    {
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #4CAF50;'>Welcome to FarmSync!</h2>
                    <p>Hello {userName},</p>
                    <p>Your employee account has been created successfully. You can now access the FarmSync system.</p>
                    <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <h3 style='margin-top: 0;'>Login Credentials:</h3>
                        <p><strong>Email:</strong> {to}</p>
                        <p><strong>Temporary Password:</strong> <span style='font-size: 18px; color: #4CAF50; font-weight: bold;'>{temporaryPassword}</span></p>
                    </div>
                    <p style='color: #ff9800;'>⚠️ <strong>Important:</strong> Please change your password after your first login for security.</p>
                    <p>Login at: <a href='http://localhost:4200/login' style='color: #4CAF50; font-weight: bold;'>FarmSync Portal</a></p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated message from FarmSync. Please do not reply to this email.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, "Welcome to FarmSync - Your Login Credentials", body);
    }

    public async Task SendDocumentExpiryWarningAsync(string to, string documentType, DateTime expiryDate, int daysUntilExpiry)
    {
        var urgencyColor = daysUntilExpiry <= 7 ? "#f44336" : "#ff9800";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: {urgencyColor};'>⚠️ Document Expiring Soon</h2>
                    <p>Your <strong>{documentType}</strong> will expire in <strong style='color: {urgencyColor};'>{daysUntilExpiry} days</strong>.</p>
                    <div style='background-color: #fff3cd; padding: 15px; border-left: 4px solid {urgencyColor}; margin: 20px 0;'>
                        <p style='margin: 0;'><strong>Expiry Date:</strong> {expiryDate:dd MMM yyyy}</p>
                    </div>
                    <p>Please renew this document as soon as possible to avoid any disruptions.</p>
                    <p>Login to FarmSync to upload the renewed document.</p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated reminder from FarmSync.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, $"Document Expiry Warning - {documentType}", body);
    }

    public async Task SendDocumentExpiredNotificationAsync(string to, string documentType, DateTime expiryDate)
    {
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #f44336;'>⛔ Document Expired</h2>
                    <p>Your <strong>{documentType}</strong> has expired.</p>
                    <div style='background-color: #ffebee; padding: 15px; border-left: 4px solid #f44336; margin: 20px 0;'>
                        <p style='margin: 0;'><strong>Expired On:</strong> {expiryDate:dd MMM yyyy}</p>
                    </div>
                    <p style='color: #f44336; font-weight: bold;'>⚠️ URGENT: Please renew this document immediately.</p>
                    <p>Login to FarmSync to upload the renewed document.</p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated notification from FarmSync.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, $"URGENT: {documentType} Expired", body);
    }

    public async Task SendPurchaseOrderStatusChangeAsync(string to, string poNumber, string oldStatus, string newStatus)
    {
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #2196F3;'>Purchase Order Status Update</h2>
                    <p>Purchase Order <strong>{poNumber}</strong> status has been updated.</p>
                    <div style='background-color: #e3f2fd; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Previous Status:</strong> {oldStatus}</p>
                        <p><strong>New Status:</strong> {newStatus}</p>
                    </div>
                    <p>Login to FarmSync to view details.</p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated notification from FarmSync.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, $"PO {poNumber} Status Changed", body);
    }

    public async Task SendMaintenanceDueNotificationAsync(string to, string vehicleRegistration, DateTime dueDate)
    {
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #ff9800;'>🔧 Maintenance Due</h2>
                    <p>Vehicle <strong>{vehicleRegistration}</strong> is due for maintenance.</p>
                    <div style='background-color: #fff3e0; padding: 15px; border-left: 4px solid #ff9800; margin: 20px 0;'>
                        <p style='margin: 0;'><strong>Due Date:</strong> {dueDate:dd MMM yyyy}</p>
                    </div>
                    <p>Please schedule the maintenance to keep the vehicle in optimal condition.</p>
                    <p>Login to FarmSync to create a maintenance record.</p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated reminder from FarmSync.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, $"Maintenance Due - {vehicleRegistration}", body);
    }

    public async Task SendLeaveApprovalNotificationAsync(string to, string leaveType, bool approved, string? reason = null)
    {
        var statusColor = approved ? "#4CAF50" : "#f44336";
        var statusText = approved ? "Approved" : "Rejected";
        var statusIcon = approved ? "✅" : "❌";
        
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <div style='max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: {statusColor};'>{statusIcon} Leave Request {statusText}</h2>
                    <p>Your <strong>{leaveType}</strong> leave request has been <strong>{statusText.ToLower()}</strong>.</p>
                    {(reason != null ? $"<div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;'><p><strong>Reason:</strong> {reason}</p></div>" : "")}
                    <p>Login to FarmSync for more details.</p>
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #ddd;'>
                    <p style='color: #888; font-size: 12px;'>This is an automated notification from FarmSync.</p>
                </div>
            </body>
            </html>";

        await SendEmailAsync(to, $"Leave Request {statusText}", body);
    }
}
