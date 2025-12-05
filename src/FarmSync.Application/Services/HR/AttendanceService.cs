using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class AttendanceService : IAttendanceService
{
    private readonly IClockEventRepository _clockEventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AttendanceService(IClockEventRepository clockEventRepository, IUnitOfWork unitOfWork)
    {
        _clockEventRepository = clockEventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ClockEventDTO> RecordClockEventAsync(CreateClockEventDTO dto)
    {
        var clockEvent = new ClockEvent
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            TeamId = dto.TeamId,
            EventTime = dto.EventTime,
            EventType = dto.EventType,
            BiometricId = dto.BiometricId,
            Location = dto.Location,
            Notes = dto.Notes
        };

        await _clockEventRepository.AddAsync(clockEvent);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(clockEvent);
    }

    public async Task<IEnumerable<ClockEventDTO>> GetEmployeeAttendanceAsync(Guid employeeId, DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        var events = await _clockEventRepository.GetByEmployeeAsync(employeeId, fromDate, toDate);
        return events.Select(MapToDto);
    }

    public async Task<IEnumerable<ClockEventDTO>> GetTeamAttendanceAsync(Guid teamId, DateOnly date)
    {
        var events = await _clockEventRepository.GetByTeamAsync(teamId, date);
        return events.Select(MapToDto);
    }

    public async Task<AttendanceSummaryDTO> GetDailySummaryAsync(Guid employeeId, DateOnly date)
    {
        var events = await _clockEventRepository.GetByEmployeeAsync(employeeId, date, date);
        var eventList = events.ToList();

        var clockIn = eventList.FirstOrDefault(e => e.EventType == "ClockIn");
        var clockOut = eventList.FirstOrDefault(e => e.EventType == "ClockOut");

        decimal? totalHours = null;
        if (clockIn != null && clockOut != null)
        {
            totalHours = (decimal)(clockOut.EventTime - clockIn.EventTime).TotalHours;
        }

        return new AttendanceSummaryDTO
        {
            EmployeeId = employeeId,
            EmployeeName = clockIn?.Employee?.FullName ?? "",
            Date = date,
            ClockIn = clockIn?.EventTime,
            ClockOut = clockOut?.EventTime,
            TotalHours = totalHours,
            IsPresent = clockIn != null,
            Events = eventList.Select(MapToDto).ToList()
        };
    }

    public async Task<IEnumerable<AttendanceSummaryDTO>> GetTeamDailySummaryAsync(Guid teamId, DateOnly date)
    {
        var events = await _clockEventRepository.GetByTeamAsync(teamId, date);
        var groupedByEmployee = events.GroupBy(e => e.EmployeeId);

        var summaries = new List<AttendanceSummaryDTO>();
        foreach (var group in groupedByEmployee)
        {
            var eventList = group.ToList();
            var clockIn = eventList.FirstOrDefault(e => e.EventType == "ClockIn");
            var clockOut = eventList.FirstOrDefault(e => e.EventType == "ClockOut");

            decimal? totalHours = null;
            if (clockIn != null && clockOut != null)
            {
                totalHours = (decimal)(clockOut.EventTime - clockIn.EventTime).TotalHours;
            }

            summaries.Add(new AttendanceSummaryDTO
            {
                EmployeeId = group.Key,
                EmployeeName = clockIn?.Employee?.FullName ?? "",
                Date = date,
                ClockIn = clockIn?.EventTime,
                ClockOut = clockOut?.EventTime,
                TotalHours = totalHours,
                IsPresent = clockIn != null,
                Events = eventList.Select(MapToDto).ToList()
            });
        }

        return summaries;
    }

    public async Task<ClockEventDTO?> GetLastEventAsync(Guid employeeId)
    {
        var clockEvent = await _clockEventRepository.GetLastEventAsync(employeeId);
        return clockEvent != null ? MapToDto(clockEvent) : null;
    }

    private static ClockEventDTO MapToDto(ClockEvent clockEvent)
    {
        return new ClockEventDTO
        {
            Id = clockEvent.Id,
            EmployeeId = clockEvent.EmployeeId,
            EmployeeName = clockEvent.Employee?.FullName ?? "",
            EmployeeNumber = clockEvent.Employee?.EmployeeNumber ?? "",
            TeamId = clockEvent.TeamId,
            TeamName = clockEvent.Team?.Name,
            EventTime = clockEvent.EventTime,
            EventType = clockEvent.EventType,
            BiometricId = clockEvent.BiometricId,
            Location = clockEvent.Location,
            Notes = clockEvent.Notes,
            CreatedAt = clockEvent.CreatedAt
        };
    }
}
