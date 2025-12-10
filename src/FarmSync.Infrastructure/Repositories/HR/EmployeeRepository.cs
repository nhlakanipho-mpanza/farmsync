using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public override async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.EmploymentType)
            .Include(e => e.BiometricEnrolment)
            .Include(e => e.BankDetails)
                .ThenInclude(b => b!.BankName)
            .Include(e => e.BankDetails)
                .ThenInclude(b => b!.AccountType)
            .Include(e => e.EmergencyContacts)
            .Include(e => e.DriverLicenseDocument)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.EmploymentType)
            .Include(e => e.BiometricEnrolment)
            .Include(e => e.BankDetails)
                .ThenInclude(b => b!.BankName)
            .Include(e => e.BankDetails)
                .ThenInclude(b => b!.AccountType)
            .Include(e => e.EmergencyContacts)
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.EmploymentType)
            .Where(e => e.IsActive)
            .OrderBy(e => e.FullName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByPositionAsync(Guid positionId)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.EmploymentType)
            .Where(e => e.PositionId == positionId)
            .OrderBy(e => e.FullName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByEmploymentTypeAsync(Guid employmentTypeId)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.EmploymentType)
            .Where(e => e.EmploymentTypeId == employmentTypeId)
            .OrderBy(e => e.FullName)
            .ToListAsync();
    }

    public async Task<bool> EmployeeNumberExistsAsync(string employeeNumber)
    {
        return await _context.Employees
            .AnyAsync(e => e.EmployeeNumber == employeeNumber);
    }
}
