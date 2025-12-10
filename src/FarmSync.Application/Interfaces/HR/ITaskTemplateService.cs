using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface ITaskTemplateService
{
    Task<IEnumerable<TaskTemplateDTO>> GetAllTemplatesAsync();
    Task<IEnumerable<TaskTemplateDTO>> GetActiveTemplatesAsync();
    Task<IEnumerable<TaskTemplateDTO>> GetByCategoryAsync(string category);
    Task<IEnumerable<TaskTemplateDTO>> GetRecurringTemplatesAsync();
    Task<TaskTemplateDTO?> GetTemplateByIdAsync(Guid id);
    Task<TaskTemplateDTO> CreateTemplateAsync(CreateTaskTemplateDTO dto);
    Task<TaskTemplateDTO> UpdateTemplateAsync(Guid id, UpdateTaskTemplateDTO dto);
    Task DeleteTemplateAsync(Guid id);
}
