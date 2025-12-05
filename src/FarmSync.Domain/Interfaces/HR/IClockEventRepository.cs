using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface IClockEventRepository : IRepository<ClockEvent>
{
    Task<IEnumerable<ClockEvent>> GetByEmployeeAsync(Guid employeeId, DateOnly? fromDate = null, DateOnly? toDate = null);
    Task<IEnumerable<ClockEvent>> GetByTeamAsync(Guid teamId, DateOnly date);
    Task<ClockEvent?> GetLastEventAsync(Guid employeeId);
    Task<IEnumerable<ClockEvent>> GetEventsByDateAsync(DateOnly date);
}
