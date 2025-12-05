using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpPost("clock")]
    public async Task<ActionResult<ClockEventDTO>> RecordClockEvent([FromBody] CreateClockEventDTO dto)
    {
        var clockEvent = await _attendanceService.RecordClockEventAsync(dto);
        return Ok(clockEvent);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<ClockEventDTO>>> GetEmployeeAttendance(
        Guid employeeId, 
        [FromQuery] DateOnly? fromDate = null, 
        [FromQuery] DateOnly? toDate = null)
    {
        var events = await _attendanceService.GetEmployeeAttendanceAsync(employeeId, fromDate, toDate);
        return Ok(events);
    }

    [HttpGet("team/{teamId}/{date}")]
    public async Task<ActionResult<IEnumerable<ClockEventDTO>>> GetTeamAttendance(Guid teamId, DateOnly date)
    {
        var events = await _attendanceService.GetTeamAttendanceAsync(teamId, date);
        return Ok(events);
    }

    [HttpGet("employee/{employeeId}/summary/{date}")]
    public async Task<ActionResult<AttendanceSummaryDTO>> GetDailySummary(Guid employeeId, DateOnly date)
    {
        var summary = await _attendanceService.GetDailySummaryAsync(employeeId, date);
        return Ok(summary);
    }

    [HttpGet("team/{teamId}/summary/{date}")]
    public async Task<ActionResult<IEnumerable<AttendanceSummaryDTO>>> GetTeamDailySummary(Guid teamId, DateOnly date)
    {
        var summaries = await _attendanceService.GetTeamDailySummaryAsync(teamId, date);
        return Ok(summaries);
    }

    [HttpGet("employee/{employeeId}/last-event")]
    public async Task<ActionResult<ClockEventDTO>> GetLastEvent(Guid employeeId)
    {
        var clockEvent = await _attendanceService.GetLastEventAsync(employeeId);
        if (clockEvent == null)
        {
            return NotFound($"No clock events found for employee {employeeId}.");
        }
        return Ok(clockEvent);
    }
}
