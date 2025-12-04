using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(suppliers);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActive()
    {
        var suppliers = await _supplierService.GetActiveAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound($"Supplier with ID {id} not found.");
        }
        return Ok(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var supplier = await _supplierService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<SupplierDto>> Update(Guid id, [FromBody] CreateSupplierDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var supplier = await _supplierService.UpdateAsync(id, dto, userId);
            return Ok(supplier);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _supplierService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
