using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
    Task<IEnumerable<EmployeeDTO>> GetActiveEmployeesAsync();
    Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDTO?> GetEmployeeByNumberAsync(string employeeNumber);
    Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO dto);
    Task<EmployeeDTO> UpdateEmployeeAsync(Guid id, UpdateEmployeeDTO dto);
    Task DeleteEmployeeAsync(Guid id);
    Task<BankDetailsDTO> AddBankDetailsAsync(CreateBankDetailsDTO dto);
    Task<BankDetailsDTO> UpdateBankDetailsAsync(Guid employeeId, UpdateBankDetailsDTO dto);
    Task<BiometricEnrolmentDTO> EnrollBiometricAsync(CreateBiometricEnrolmentDTO dto);
    Task<EmergencyContactDTO> AddEmergencyContactAsync(CreateEmergencyContactDTO dto);
    Task DeleteEmergencyContactAsync(Guid id);
}
