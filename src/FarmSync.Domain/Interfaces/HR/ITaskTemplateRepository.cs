using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface ITaskTemplateRepository
{
    Task<IEnumerable<TaskTemplate>> GetAllAsync();
    Task<IEnumerable<TaskTemplate>> GetActivTemplatesAsync();
    Task<IEnumerable<TaskTemplate>> GetByCategoryAsync(string category);
    Task<IEnumerable<TaskTemplate>> GetRecurringTemplatesAsync();
    Task<TaskTemplate?> GetByIdAsync(Guid id);
    Task<TaskTemplate?> GetByIdWithChecklistAsync(Guid id);
    Task<TaskTemplate> CreateAsync(TaskTemplate template);
    Task UpdateAsync(TaskTemplate template);
    Task DeleteAsync(Guid id);
}
