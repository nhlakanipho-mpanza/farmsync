using FarmSync.Application.DTOs.Notifications;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(
        INotificationService notificationService,
        ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetUserNotifications([FromQuery] bool unreadOnly = false)
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("User ID not found in token");

        var notifications = await _notificationService.GetUserNotificationsAsync(userId, unreadOnly);
        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<UnreadCountDto>> GetUnreadCount()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("User ID not found in token");

        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(new UnreadCountDto { UnreadCount = count });
    }

    [HttpPut("{id:guid}/mark-read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("User ID not found in token");

        await _notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NotificationDto>> CreateNotification([FromBody] CreateNotificationDto dto)
    {
        try
        {
            var notification = await _notificationService.CreateNotificationAsync(dto);
            return Ok(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification");
            return StatusCode(500, "An error occurred while creating the notification");
        }
    }

    [HttpPost("send")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SendNotification([FromBody] CreateNotificationDto dto)
    {
        try
        {
            if (!Enum.TryParse<NotificationType>(dto.Type, out var notificationType))
                return BadRequest("Invalid notification type");

            await _notificationService.SendNotificationAsync(
                dto.UserId,
                notificationType,
                dto.Title,
                dto.Message,
                dto.ActionUrl,
                dto.Data);

            return Ok(new { message = "Notification sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification");
            return StatusCode(500, "An error occurred while sending the notification");
        }
    }

    [HttpGet("settings")]
    public async Task<ActionResult<NotificationSettingDto>> GetSettings()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("User ID not found in token");

        var settings = await _notificationService.GetUserSettingsAsync(userId);
        if (settings == null)
            return NotFound();

        return Ok(settings);
    }

    [HttpPut("settings")]
    public async Task<ActionResult<NotificationSettingDto>> UpdateSettings([FromBody] UpdateNotificationSettingDto dto)
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("User ID not found in token");

        var settings = await _notificationService.UpdateUserSettingsAsync(userId, dto);
        return Ok(settings);
    }
}
