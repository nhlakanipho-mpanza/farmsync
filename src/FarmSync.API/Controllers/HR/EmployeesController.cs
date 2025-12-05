using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetActive()
    {
        var employees = await _employeeService.GetActiveEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDTO>> GetById(Guid id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found.");
        }
        return Ok(employee);
    }

    [HttpGet("number/{employeeNumber}")]
    public async Task<ActionResult<EmployeeDTO>> GetByNumber(string employeeNumber)
    {
        var employee = await _employeeService.GetEmployeeByNumberAsync(employeeNumber);
        if (employee == null)
        {
            return NotFound($"Employee with number {employeeNumber} not found.");
        }
        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<EmployeeDTO>> Create([FromBody] CreateEmployeeDTO dto)
    {
        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<EmployeeDTO>> Update(Guid id, [FromBody] UpdateEmployeeDTO dto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(employee);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{employeeId}/bank-details")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<BankDetailsDTO>> AddBankDetails([FromBody] CreateBankDetailsDTO dto)
    {
        var bankDetails = await _employeeService.AddBankDetailsAsync(dto);
        return Ok(bankDetails);
    }

    [HttpPut("{employeeId}/bank-details")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<BankDetailsDTO>> UpdateBankDetails(Guid employeeId, [FromBody] UpdateBankDetailsDTO dto)
    {
        try
        {
            var bankDetails = await _employeeService.UpdateBankDetailsAsync(employeeId, dto);
            return Ok(bankDetails);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{employeeId}/biometric")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<BiometricEnrolmentDTO>> EnrollBiometric([FromBody] CreateBiometricEnrolmentDTO dto)
    {
        var enrolment = await _employeeService.EnrollBiometricAsync(dto);
        return Ok(enrolment);
    }

    [HttpPost("{employeeId}/emergency-contacts")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<EmergencyContactDTO>> AddEmergencyContact([FromBody] CreateEmergencyContactDTO dto)
    {
        var contact = await _employeeService.AddEmergencyContactAsync(dto);
        return Ok(contact);
    }

    [HttpDelete("emergency-contacts/{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult> DeleteEmergencyContact(Guid id)
    {
        await _employeeService.DeleteEmergencyContactAsync(id);
        return NoContent();
    }
}
