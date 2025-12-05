namespace FarmSync.Application.DTOs.Fleet;

public class VehicleDTO
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? EngineNumber { get; set; }
    public string? ChassisNumber { get; set; }
    public string? AssetNumber { get; set; }
    public int CurrentOdometer { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    
    // Maintenance Tracking
    public DateTime? LastServiceDate { get; set; }
    public int? LastServiceOdometer { get; set; }
    public string? LastServiceType { get; set; }
    public int? NextServiceOdometer { get; set; }
    
    // License Disk Renewal
    public DateTime? LicenseDiskExpiryDate { get; set; }
    
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    // Foreign Keys
    public Guid VehicleTypeId { get; set; }
    public Guid VehicleStatusId { get; set; }
    public Guid FuelTypeId { get; set; }

    // Display Names
    public string? VehicleTypeName { get; set; }
    public string? VehicleStatusName { get; set; }
    public string? FuelTypeName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateVehicleDTO
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? EngineNumber { get; set; }
    public string? ChassisNumber { get; set; }
    public string? AssetNumber { get; set; }
    public int CurrentOdometer { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    
    // Maintenance Tracking
    public DateTime? LastServiceDate { get; set; }
    public int? LastServiceOdometer { get; set; }
    public string? LastServiceType { get; set; } // "Minor" or "Major"
    
    // License Disk Renewal
    public DateTime? LicenseDiskExpiryDate { get; set; }
    
    public string? Notes { get; set; }

    public Guid VehicleTypeId { get; set; }
    public Guid VehicleStatusId { get; set; }
    public Guid FuelTypeId { get; set; }
}

public class UpdateVehicleDTO
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? EngineNumber { get; set; }
    public string? ChassisNumber { get; set; }
    public string? AssetNumber { get; set; }
    public int CurrentOdometer { get; set; }
    
    // Maintenance Tracking
    public DateTime? LastServiceDate { get; set; }
    public int? LastServiceOdometer { get; set; }
    public string? LastServiceType { get; set; }
    public int? NextServiceOdometer { get; set; }
    
    // License Disk Renewal
    public DateTime? LicenseDiskExpiryDate { get; set; }
    
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    public Guid VehicleTypeId { get; set; }
    public Guid VehicleStatusId { get; set; }
    public Guid FuelTypeId { get; set; }
}
