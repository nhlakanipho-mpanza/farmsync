using FarmSync.Domain.Entities.Fleet;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IDriverAssignmentRepository : IRepository<DriverAssignment>
{
    Task<DriverAssignment?> GetCurrentAssignmentByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<DriverAssignment>> GetAssignmentsByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<DriverAssignment>> GetAssignmentsByDriverAsync(Guid driverId);
    Task<IEnumerable<DriverAssignment>> GetCurrentAssignmentsAsync();
    Task<bool> HasActiveAssignmentAsync(Guid vehicleId);
}
