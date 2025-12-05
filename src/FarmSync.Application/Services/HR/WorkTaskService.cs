using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class WorkTaskService : IWorkTaskService
{
    private readonly IWorkTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkTaskService(IWorkTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByStatusAsync(Guid statusId)
    {
        var tasks = await _taskRepository.GetByStatusAsync(statusId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByTeamAsync(Guid teamId)
    {
        var tasks = await _taskRepository.GetByTeamAsync(teamId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByEmployeeAsync(Guid employeeId)
    {
        var tasks = await _taskRepository.GetByEmployeeAsync(employeeId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetScheduledTasksAsync(DateOnly date)
    {
        var tasks = await _taskRepository.GetScheduledTasksAsync(date);
        return tasks.Select(MapToDto);
    }

    public async Task<WorkTaskDTO?> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<WorkTaskDTO> CreateTaskAsync(CreateWorkTaskDTO dto)
    {
        var task = new WorkTask
        {
            Id = Guid.NewGuid(),
            TaskDescription = dto.TaskDescription,
            WorkAreaId = dto.WorkAreaId,
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            TaskStatusId = dto.TaskStatusId,
            ScheduledDate = dto.ScheduledDate,
            EstimatedHours = dto.EstimatedHours,
            Notes = dto.Notes
        };

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<WorkTaskDTO> UpdateTaskAsync(Guid id, UpdateWorkTaskDTO dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found");
        }

        task.TaskDescription = dto.TaskDescription;
        task.WorkAreaId = dto.WorkAreaId;
        task.TeamId = dto.TeamId;
        task.EmployeeId = dto.EmployeeId;
        task.TaskStatusId = dto.TaskStatusId;
        task.ScheduledDate = dto.ScheduledDate;
        task.StartDate = dto.StartDate;
        task.CompletedDate = dto.CompletedDate;
        task.EstimatedHours = dto.EstimatedHours;
        task.ActualHours = dto.ActualHours;
        task.Notes = dto.Notes;

        await _taskRepository.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        await _taskRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static WorkTaskDTO MapToDto(WorkTask task)
    {
        return new WorkTaskDTO
        {
            Id = task.Id,
            TaskDescription = task.TaskDescription,
            WorkAreaId = task.WorkAreaId,
            WorkAreaName = task.WorkArea?.Name,
            TeamId = task.TeamId,
            TeamName = task.Team?.Name,
            EmployeeId = task.EmployeeId,
            EmployeeName = task.Employee?.FullName,
            TaskStatusId = task.TaskStatusId,
            TaskStatusName = task.TaskStatus?.Name ?? "",
            ScheduledDate = task.ScheduledDate,
            StartDate = task.StartDate,
            CompletedDate = task.CompletedDate,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            Notes = task.Notes,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
