namespace FarmSync.Application.DTOs.Fleet;

public class FuelLogDTO
{
    public Guid Id { get; set; }
    public DateTime FuelDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
    public int OdometerReading { get; set; }
    public string? Station { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsFull { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    public Guid VehicleId { get; set; }
    public Guid? FilledById { get; set; }

    public string? VehicleRegistration { get; set; }
    public string? FilledByName { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CreateFuelLogDTO
{
    public DateTime FuelDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int OdometerReading { get; set; }
    public string? Station { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsFull { get; set; } = true;
    public string? Notes { get; set; }

    public Guid VehicleId { get; set; }
    public Guid? FilledById { get; set; }
}

public class UpdateFuelLogDTO
{
    public DateTime FuelDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int OdometerReading { get; set; }
    public string? Station { get; set; }
    public string? ReceiptNumber { get; set; }
    public bool IsFull { get; set; }
    public string? Notes { get; set; }
}
