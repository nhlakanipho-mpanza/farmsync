using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class DriverAssignmentService : IDriverAssignmentService
{
    private readonly IDriverAssignmentRepository _assignmentRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DriverAssignmentService(
        IDriverAssignmentRepository assignmentRepository,
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork)
    {
        _assignmentRepository = assignmentRepository;
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DriverAssignmentDTO> AssignDriverToVehicleAsync(CreateDriverAssignmentDTO dto, Guid assignedById)
    {
        // Verify vehicle exists
        var vehicle = await _vehicleRepository.GetByIdAsync(dto.VehicleId);
        if (vehicle == null)
            throw new KeyNotFoundException($"Vehicle with ID {dto.VehicleId} not found");

        // End any existing current assignment for this vehicle if this is a primary assignment
        if (dto.IsPrimary)
        {
            var existingAssignment = await _assignmentRepository.GetCurrentAssignmentByVehicleAsync(dto.VehicleId);
            if (existingAssignment != null)
            {
                existingAssignment.EndDate = dto.StartDate.AddSeconds(-1);
                await _assignmentRepository.UpdateAsync(existingAssignment);
            }
        }

        // Create new assignment
        var assignment = new DriverAssignment
        {
            VehicleId = dto.VehicleId,
            DriverId = dto.DriverId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            AssignmentType = dto.AssignmentType,
            IsPrimary = dto.IsPrimary,
            Notes = dto.Notes,
            AssignedById = assignedById
        };

        await _assignmentRepository.AddAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        return await GetAssignmentByIdAsync(assignment.Id);
    }

    public async Task<DriverAssignmentDTO> UpdateAssignmentAsync(Guid id, UpdateDriverAssignmentDTO dto)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
            throw new KeyNotFoundException($"Assignment with ID {id} not found");

        if (dto.EndDate.HasValue)
        {
            assignment.EndDate = dto.EndDate.Value;
        }

        if (dto.Notes != null)
        {
            assignment.Notes = dto.Notes;
        }

        await _assignmentRepository.UpdateAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        return await GetAssignmentByIdAsync(id);
    }

    public async Task EndAssignmentAsync(Guid id, DateTime endDate)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
            throw new KeyNotFoundException($"Assignment with ID {id} not found");

        assignment.EndDate = endDate;

        await _assignmentRepository.UpdateAsync(assignment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAssignmentAsync(Guid id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
            throw new KeyNotFoundException($"Assignment with ID {id} not found");

        await _assignmentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<DriverAssignmentDTO?> GetCurrentAssignmentByVehicleAsync(Guid vehicleId)
    {
        var assignment = await _assignmentRepository.GetCurrentAssignmentByVehicleAsync(vehicleId);
        return assignment != null ? MapToDto(assignment) : null;
    }

    public async Task<IEnumerable<DriverAssignmentDTO>> GetAssignmentsByVehicleAsync(Guid vehicleId)
    {
        var assignments = await _assignmentRepository.GetAssignmentsByVehicleAsync(vehicleId);
        return assignments.Select(MapToDto);
    }

    public async Task<IEnumerable<DriverAssignmentDTO>> GetAssignmentsByDriverAsync(Guid driverId)
    {
        var assignments = await _assignmentRepository.GetAssignmentsByDriverAsync(driverId);
        return assignments.Select(MapToDto);
    }

    public async Task<IEnumerable<DriverAssignmentDTO>> GetCurrentAssignmentsAsync()
    {
        var assignments = await _assignmentRepository.GetCurrentAssignmentsAsync();
        return assignments.Select(MapToDto);
    }

    public async Task<DriverAssignmentDTO> GetAssignmentByIdAsync(Guid id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
            throw new KeyNotFoundException($"Assignment with ID {id} not found");

        return MapToDto(assignment);
    }

    private DriverAssignmentDTO MapToDto(DriverAssignment assignment)
    {
        return new DriverAssignmentDTO
        {
            Id = assignment.Id,
            VehicleId = assignment.VehicleId,
            VehicleRegistration = assignment.Vehicle?.RegistrationNumber,
            VehicleMake = assignment.Vehicle?.Make,
            VehicleModel = assignment.Vehicle?.Model,
            DriverId = assignment.DriverId,
            DriverName = assignment.Driver?.FullName,
            DriverEmployeeNumber = assignment.Driver?.EmployeeNumber,
            StartDate = assignment.StartDate,
            EndDate = assignment.EndDate,
            AssignmentType = assignment.AssignmentType,
            IsPrimary = assignment.IsPrimary,
            Notes = assignment.Notes,
            IsCurrentAssignment = assignment.IsCurrentAssignment,
            AssignedById = assignment.AssignedById,
            AssignedByName = assignment.AssignedBy?.FullName,
            CreatedAt = assignment.CreatedAt,
            UpdatedAt = assignment.UpdatedAt
        };
    }
}
