using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.ReferenceData;

namespace FarmSync.Domain.Entities.Documents;

public class Document : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty; // Relative path on SFTP
    public string FileUrl { get; set; } = string.Empty; // Full public URL (for profile pictures only)
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    
    // Polymorphic relationship
    public string EntityType { get; set; } = string.Empty; // PurchaseOrder, Vehicle, Employee, etc.
    public Guid EntityId { get; set; }
    
    // Document Type (reference data)
    public Guid DocumentTypeId { get; set; }
    public virtual DocumentType DocumentType { get; set; } = null!;
    
    public DateTime? ExpiryDate { get; set; }
    
    public Guid UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
    
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}
