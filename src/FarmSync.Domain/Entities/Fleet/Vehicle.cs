using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class Vehicle : BaseEntity
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
    public int? NextServiceOdometer { get; set; }
    
    // License Disk Renewal
    public DateTime? LicenseDiskExpiryDate { get; set; }
    
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleTypeId { get; set; }
    public Guid VehicleStatusId { get; set; }
    public Guid FuelTypeId { get; set; }

    // Navigation Properties
    public virtual VehicleType VehicleType { get; set; } = null!;
    public virtual VehicleStatus VehicleStatus { get; set; } = null!;
    public virtual FuelType FuelType { get; set; } = null!;

    public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    public virtual ICollection<TripLog> TripLogs { get; set; } = new List<TripLog>();
    public virtual ICollection<FuelLog> FuelLogs { get; set; } = new List<FuelLog>();
    public virtual ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
    public virtual ICollection<DriverAssignment> DriverAssignments { get; set; } = new List<DriverAssignment>();
    public virtual ICollection<GPSLocation> GPSLocations { get; set; } = new List<GPSLocation>();
    public virtual ICollection<SpeedingEvent> SpeedingEvents { get; set; } = new List<SpeedingEvent>();
}
