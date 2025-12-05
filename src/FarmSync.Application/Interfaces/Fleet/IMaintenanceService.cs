using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface IMaintenanceService
{
    Task<IEnumerable<MaintenanceRecordDTO>> GetAllMaintenanceAsync();
    Task<MaintenanceRecordDTO?> GetMaintenanceByIdAsync(Guid id);
    Task<IEnumerable<MaintenanceRecordDTO>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<MaintenanceRecordDTO>> GetPendingMaintenanceAsync();
    Task<IEnumerable<MaintenanceRecordDTO>> GetCompletedMaintenanceAsync(DateTime startDate, DateTime endDate);
    Task<MaintenanceRecordDTO> CreateMaintenanceAsync(CreateMaintenanceRecordDTO dto);
    Task<MaintenanceRecordDTO> UpdateMaintenanceAsync(Guid id, UpdateMaintenanceRecordDTO dto);
    Task DeleteMaintenanceAsync(Guid id);
    Task AutoScheduleMaintenanceAsync(Guid vehicleId);
}
