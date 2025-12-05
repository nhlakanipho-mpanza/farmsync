using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IIncidentReportRepository : IRepository<IncidentReport>
{
    Task<IEnumerable<IncidentReport>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<IncidentReport>> GetByStatusAsync(string status);
    Task<IEnumerable<IncidentReport>> GetByDriverAsync(Guid driverId);
    Task<IEnumerable<IncidentReport>> GetBySeverityAsync(string severity);
    Task<IEnumerable<IncidentReport>> GetUnresolvedIncidentsAsync();
    Task<IEnumerable<IncidentReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
