using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface IWorkTaskService
{
    Task<IEnumerable<WorkTaskDTO>> GetAllTasksAsync();
    Task<IEnumerable<WorkTaskDTO>> GetTasksByStatusAsync(Guid statusId);
    Task<IEnumerable<WorkTaskDTO>> GetTasksByTeamAsync(Guid teamId);
    Task<IEnumerable<WorkTaskDTO>> GetTasksByEmployeeAsync(Guid employeeId);
    Task<IEnumerable<WorkTaskDTO>> GetScheduledTasksAsync(DateOnly date);
    Task<WorkTaskDTO?> GetTaskByIdAsync(Guid id);
    Task<WorkTaskDTO> CreateTaskAsync(CreateWorkTaskDTO dto);
    Task<WorkTaskDTO> UpdateTaskAsync(Guid id, UpdateWorkTaskDTO dto);
    Task DeleteTaskAsync(Guid id);
}
