using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;
using System.Text;

namespace FarmSync.Application.Services.HR;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRepository<EmergencyContact> _emergencyContactRepository;
    private readonly IRepository<BankDetails> _bankDetailsRepository;
    private readonly IRepository<BiometricEnrolment> _biometricRepository;
    private readonly IRepository<Position> _positionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IRepository<EmergencyContact> emergencyContactRepository,
        IRepository<BankDetails> bankDetailsRepository,
        IRepository<BiometricEnrolment> biometricRepository,
        IRepository<Position> positionRepository,
        IUserRepository userRepository,
        IRepository<UserRole> userRoleRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _emergencyContactRepository = emergencyContactRepository;
        _bankDetailsRepository = bankDetailsRepository;
        _biometricRepository = biometricRepository;
        _positionRepository = positionRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _emailService = emailService;
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

        // Check if position requires driver license
        var position = await _positionRepository.GetByIdAsync(dto.PositionId);
        if (position == null)
        {
            throw new InvalidOperationException($"Position with ID {dto.PositionId} not found");
        }

        if (position.IsDriverPosition)
        {
            if (!dto.DriverLicenseExpiryDate.HasValue)
            {
                throw new InvalidOperationException("Driver license expiry date is required for driver positions");
            }
        }

        // Generate username and password
        var username = GenerateUsername(dto.FullName, dto.EmployeeNumber);
        var temporaryPassword = GenerateTemporaryPassword();
        var passwordHash = HashPassword(temporaryPassword);

        // Create user account
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            FirstName = dto.FullName.Split(' ')[0],
            LastName = dto.FullName.Split(' ').Length > 1 ? dto.FullName.Split(' ')[^1] : "",
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Assign role to user
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = dto.RoleId
        };
        await _userRoleRepository.AddAsync(userRole);

        // Create employee
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
            UserId = user.Id,
            RoleId = dto.RoleId,
            ProfilePicture = dto.ProfilePicture,
            DriverLicenseExpiryDate = dto.DriverLicenseExpiryDate,
            DriverLicenseDocumentId = dto.DriverLicenseDocumentId,
            IsActive = true
        };

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        // Send welcome email with credentials
        try
        {
            await _emailService.SendWelcomeEmailAsync(dto.Email, username, temporaryPassword);
        }
        catch
        {
            // Don't fail the entire operation if email fails
        }

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
        employee.RoleId = dto.RoleId;
        employee.IsActive = dto.IsActive;
        employee.ProfilePicture = dto.ProfilePicture;
        employee.DriverLicenseExpiryDate = dto.DriverLicenseExpiryDate;
        employee.DriverLicenseDocumentId = dto.DriverLicenseDocumentId;

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
            PositionIsDriverPosition = employee.Position?.IsDriverPosition,
            EmploymentTypeId = employee.EmploymentTypeId,
            EmploymentTypeName = employee.EmploymentType?.Name,
            UserId = employee.UserId,
            RoleId = employee.RoleId,
            DriverLicenseExpiryDate = employee.DriverLicenseExpiryDate,
            DriverLicenseDocumentId = employee.DriverLicenseDocumentId,
            
            // Bank Details
            BankDetails = employee.BankDetails != null ? new BankDetailsDTO
            {
                Id = employee.BankDetails.Id,
                EmployeeId = employee.BankDetails.EmployeeId,
                AccountHolder = employee.BankDetails.AccountHolder,
                AccountNumber = employee.BankDetails.AccountNumber,
                BankNameId = employee.BankDetails.BankNameId,
                BankName = employee.BankDetails.BankName?.Name,
                AccountTypeId = employee.BankDetails.AccountTypeId,
                AccountType = employee.BankDetails.AccountType?.Name,
                BranchCode = employee.BankDetails.BranchCode
            } : null,
            
            // Emergency Contacts
            EmergencyContacts = employee.EmergencyContacts?.Select(ec => new EmergencyContactDTO
            {
                Id = ec.Id,
                EmployeeId = ec.EmployeeId,
                FullName = ec.FullName,
                ContactNumber = ec.ContactNumber,
                Relationship = ec.Relationship,
                AlternateNumber = ec.AlternateNumber,
                Address = ec.Address
            }).ToList() ?? new List<EmergencyContactDTO>(),
            
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    private static string GenerateUsername(string fullName, string employeeNumber)
    {
        // Generate username from first name + last initial + employee number
        var names = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var firstName = names[0].ToLower();
        var lastInitial = names.Length > 1 ? names[^1][0].ToString().ToLower() : "";
        var empNum = employeeNumber.Replace("-", "").Replace(" ", "");
        return $"{firstName}{lastInitial}{empNum}";
    }

    private static string GenerateTemporaryPassword()
    {
        // Generate random 6-character password with uppercase, lowercase, and numbers
        const string uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lowercase = "abcdefghjkmnpqrstuvwxyz";
        const string numbers = "23456789";
        
        var random = new Random();
        var password = new StringBuilder();
        
        // Ensure at least one of each type
        password.Append(uppercase[random.Next(uppercase.Length)]);
        password.Append(lowercase[random.Next(lowercase.Length)]);
        password.Append(numbers[random.Next(numbers.Length)]);
        
        // Fill the rest randomly (3 more characters to make 6 total)
        var allChars = uppercase + lowercase + numbers;
        for (int i = 3; i < 6; i++)
        {
            password.Append(allChars[random.Next(allChars.Length)]);
        }
        
        // Shuffle the password
        return new string(password.ToString().OrderBy(_ => random.Next()).ToArray());
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
