using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class FuelLog : BaseEntity
{
    public DateTime FuelDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
    public int OdometerReading { get; set; }
    public string? Station { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsFull { get; set; } = true;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid? FilledById { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee? FilledBy { get; set; }
}
