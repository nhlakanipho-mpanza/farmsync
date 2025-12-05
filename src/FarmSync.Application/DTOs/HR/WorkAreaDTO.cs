namespace FarmSync.Application.DTOs.HR;

public class WorkAreaDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Size { get; set; }
    public string? Unit { get; set; } // Hectares, Acres
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateWorkAreaDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal? Size { get; set; }
    public string? Unit { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
}

public class UpdateWorkAreaDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal? Size { get; set; }
    public string? Unit { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
