using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRepository<EmergencyContact> _emergencyContactRepository;
    private readonly IRepository<BankDetails> _bankDetailsRepository;
    private readonly IRepository<BiometricEnrolment> _biometricRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IRepository<EmergencyContact> emergencyContactRepository,
        IRepository<BankDetails> bankDetailsRepository,
        IRepository<BiometricEnrolment> biometricRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _emergencyContactRepository = emergencyContactRepository;
        _bankDetailsRepository = bankDetailsRepository;
        _biometricRepository = biometricRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeDTO>> GetActiveEmployeesAsync()
    {
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee != null ? MapToDto(employee) : null;
    }

    public async Task<EmployeeDTO?> GetEmployeeByNumberAsync(string employeeNumber)
    {
        var employee = await _employeeRepository.GetByEmployeeNumberAsync(employeeNumber);
        return employee != null ? MapToDto(employee) : null;
    }

    public async Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO dto)
    {
        if (await _employeeRepository.EmployeeNumberExistsAsync(dto.EmployeeNumber))
        {
            throw new InvalidOperationException($"Employee number {dto.EmployeeNumber} already exists");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            EmployeeNumber = dto.EmployeeNumber,
            ContactNumber = dto.ContactNumber,
            Email = dto.Email,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            HireDate = dto.HireDate,
            IdNumber = dto.IdNumber,
            Gender = dto.Gender,
            PositionId = dto.PositionId,
            EmploymentTypeId = dto.EmploymentTypeId,
            RoleTypeId = dto.RoleTypeId,
            ProfilePicture = dto.ProfilePicture,
            IsActive = true
        };

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task<EmployeeDTO> UpdateEmployeeAsync(Guid id, UpdateEmployeeDTO dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }

        employee.FullName = dto.FullName;
        employee.ContactNumber = dto.ContactNumber;
        employee.Email = dto.Email;
        employee.Address = dto.Address;
        employee.DateOfBirth = dto.DateOfBirth;
        employee.IdNumber = dto.IdNumber;
        employee.Gender = dto.Gender;
        employee.PositionId = dto.PositionId;
        employee.EmploymentTypeId = dto.EmploymentTypeId;
        employee.RoleTypeId = dto.RoleTypeId;
        employee.IsActive = dto.IsActive;
        employee.ProfilePicture = dto.ProfilePicture;

        await _employeeRepository.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task DeleteEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }

        await _employeeRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<BankDetailsDTO> AddBankDetailsAsync(CreateBankDetailsDTO dto)
    {
        var bankDetails = new BankDetails
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            AccountNumber = dto.AccountNumber,
            BankNameId = dto.BankNameId,
            AccountTypeId = dto.AccountTypeId,
            BranchCode = dto.BranchCode
        };

        await _bankDetailsRepository.AddAsync(bankDetails);
        await _unitOfWork.SaveChangesAsync();

        return new BankDetailsDTO
        {
            Id = bankDetails.Id,
            EmployeeId = bankDetails.EmployeeId,
            AccountNumber = bankDetails.AccountNumber,
            BankNameId = bankDetails.BankNameId,
            AccountTypeId = bankDetails.AccountTypeId,
            BranchCode = bankDetails.BranchCode
        };
    }

    public async Task<BankDetailsDTO> UpdateBankDetailsAsync(Guid employeeId, UpdateBankDetailsDTO dto)
    {
        var bankDetails = (await _bankDetailsRepository.GetAllAsync())
            .FirstOrDefault(b => b.EmployeeId == employeeId);

        if (bankDetails == null)
        {
            throw new KeyNotFoundException($"Bank details for employee {employeeId} not found");
        }

        bankDetails.AccountNumber = dto.AccountNumber;
        bankDetails.BankNameId = dto.BankNameId;
        bankDetails.AccountTypeId = dto.AccountTypeId;
        bankDetails.BranchCode = dto.BranchCode;

        await _bankDetailsRepository.UpdateAsync(bankDetails);
        await _unitOfWork.SaveChangesAsync();

        return new BankDetailsDTO
        {
            Id = bankDetails.Id,
            EmployeeId = bankDetails.EmployeeId,
            AccountNumber = bankDetails.AccountNumber,
            BankNameId = bankDetails.BankNameId,
            AccountTypeId = bankDetails.AccountTypeId,
            BranchCode = bankDetails.BranchCode
        };
    }

    public async Task<BiometricEnrolmentDTO> EnrollBiometricAsync(CreateBiometricEnrolmentDTO dto)
    {
        var enrolment = new BiometricEnrolment
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            BiometricId = dto.BiometricId,
            EnrolledAt = DateTime.UtcNow.AddHours(2),
            IsActive = true
        };

        await _biometricRepository.AddAsync(enrolment);
        await _unitOfWork.SaveChangesAsync();

        return new BiometricEnrolmentDTO
        {
            Id = enrolment.Id,
            EmployeeId = enrolment.EmployeeId,
            BiometricId = enrolment.BiometricId,
            EnrolledAt = enrolment.EnrolledAt,
            IsActive = enrolment.IsActive
        };
    }

    public async Task<EmergencyContactDTO> AddEmergencyContactAsync(CreateEmergencyContactDTO dto)
    {
        var contact = new EmergencyContact
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            FullName = dto.FullName,
            Relationship = dto.Relationship,
            ContactNumber = dto.ContactNumber,
            AlternateNumber = dto.AlternateNumber,
            Address = dto.Address
        };

        await _emergencyContactRepository.AddAsync(contact);
        await _unitOfWork.SaveChangesAsync();

        return new EmergencyContactDTO
        {
            Id = contact.Id,
            EmployeeId = contact.EmployeeId,
            FullName = contact.FullName,
            Relationship = contact.Relationship,
            ContactNumber = contact.ContactNumber,
            AlternateNumber = contact.AlternateNumber,
            Address = contact.Address
        };
    }

    public async Task DeleteEmergencyContactAsync(Guid id)
    {
        await _emergencyContactRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static EmployeeDTO MapToDto(Employee employee)
    {
        return new EmployeeDTO
        {
            Id = employee.Id,
            FullName = employee.FullName,
            EmployeeNumber = employee.EmployeeNumber,
            ContactNumber = employee.ContactNumber,
            Email = employee.Email,
            Address = employee.Address,
            DateOfBirth = employee.DateOfBirth,
            HireDate = employee.HireDate,
            IdNumber = employee.IdNumber,
            Gender = employee.Gender,
            IsActive = employee.IsActive,
            ProfilePicture = employee.ProfilePicture,
            PositionId = employee.PositionId,
            PositionName = employee.Position?.Name,
            EmploymentTypeId = employee.EmploymentTypeId,
            EmploymentTypeName = employee.EmploymentType?.Name,
            RoleTypeId = employee.RoleTypeId,
            RoleTypeName = employee.RoleType?.Name,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}
