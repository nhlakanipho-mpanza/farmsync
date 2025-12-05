using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface IAttendanceService
{
    Task<ClockEventDTO> RecordClockEventAsync(CreateClockEventDTO dto);
    Task<IEnumerable<ClockEventDTO>> GetEmployeeAttendanceAsync(Guid employeeId, DateOnly? fromDate = null, DateOnly? toDate = null);
    Task<IEnumerable<ClockEventDTO>> GetTeamAttendanceAsync(Guid teamId, DateOnly date);
    Task<AttendanceSummaryDTO> GetDailySummaryAsync(Guid employeeId, DateOnly date);
    Task<IEnumerable<AttendanceSummaryDTO>> GetTeamDailySummaryAsync(Guid teamId, DateOnly date);
    Task<ClockEventDTO?> GetLastEventAsync(Guid employeeId);
}
