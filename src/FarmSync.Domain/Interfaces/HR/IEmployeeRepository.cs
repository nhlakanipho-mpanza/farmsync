using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber);
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<IEnumerable<Employee>> GetByPositionAsync(Guid positionId);
    Task<IEnumerable<Employee>> GetByEmploymentTypeAsync(Guid employmentTypeId);
    Task<bool> EmployeeNumberExistsAsync(string employeeNumber);
}
