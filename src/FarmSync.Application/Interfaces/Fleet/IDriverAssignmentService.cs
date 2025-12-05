using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface IDriverAssignmentService
{
    Task<DriverAssignmentDTO> AssignDriverToVehicleAsync(CreateDriverAssignmentDTO dto, Guid assignedById);
    Task<DriverAssignmentDTO> UpdateAssignmentAsync(Guid id, UpdateDriverAssignmentDTO dto);
    Task EndAssignmentAsync(Guid id, DateTime endDate);
    Task<DriverAssignmentDTO?> GetCurrentAssignmentByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<DriverAssignmentDTO>> GetAssignmentsByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<DriverAssignmentDTO>> GetAssignmentsByDriverAsync(Guid driverId);
    Task<IEnumerable<DriverAssignmentDTO>> GetCurrentAssignmentsAsync();
    Task<DriverAssignmentDTO> GetAssignmentByIdAsync(Guid id);
    Task DeleteAssignmentAsync(Guid id);
}
