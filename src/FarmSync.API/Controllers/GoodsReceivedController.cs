using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoodsReceivedController : ControllerBase
{
    private readonly IGoodsReceivedService _service;
    private readonly ILogger<GoodsReceivedController> _logger;

    public GoodsReceivedController(IGoodsReceivedService service, ILogger<GoodsReceivedController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var goodsReceivedRecords = await _service.GetAllAsync();
            return Ok(goodsReceivedRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all goods received records");
            return StatusCode(500, new { message = "An error occurred while fetching goods received records" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var goodsReceived = await _service.GetByIdAsync(id);
            if (goodsReceived == null)
                return NotFound(new { message = $"Goods received record with ID {id} not found" });

            return Ok(goodsReceived);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting goods received record {Id}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the goods received record" });
        }
    }

    [HttpGet("purchase-order/{purchaseOrderId}")]
    public async Task<IActionResult> GetByPurchaseOrder(Guid purchaseOrderId)
    {
        try
        {
            var goodsReceivedRecords = await _service.GetByPurchaseOrderAsync(purchaseOrderId);
            return Ok(goodsReceivedRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting goods received records for PO {PurchaseOrderId}", purchaseOrderId);
            return StatusCode(500, new { message = "An error occurred while fetching goods received records" });
        }
    }

    [HttpGet("pending-approvals")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        try
        {
            var goodsReceivedRecords = await _service.GetPendingApprovalsAsync();
            return Ok(goodsReceivedRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending goods received approvals");
            return StatusCode(500, new { message = "An error occurred while fetching pending approvals" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Store Clerk,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateGoodsReceivedDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var goodsReceived = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = goodsReceived.Id }, goodsReceived);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating goods received record");
            return StatusCode(500, new { message = "An error occurred while creating the goods received record" });
        }
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var goodsReceived = await _service.ApproveAsync(id, userId);
            return Ok(goodsReceived);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving goods received record {Id}", id);
            return StatusCode(500, new { message = "An error occurred while approving the goods received record" });
        }
    }

    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
    {
        try
        {
            var goodsReceived = await _service.RejectAsync(id, reason);
            return Ok(goodsReceived);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting goods received record {Id}", id);
            return StatusCode(500, new { message = "An error occurred while rejecting the goods received record" });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }
}
