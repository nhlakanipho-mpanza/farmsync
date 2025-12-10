using System.ComponentModel.DataAnnotations;

namespace FarmSync.Application.DTOs.Documents;

public class DocumentDTO
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public Guid DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public Guid UploadedBy { get; set; }
    public string UploadedByName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}
