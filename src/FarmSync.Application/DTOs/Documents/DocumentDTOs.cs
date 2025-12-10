namespace FarmSync.Application.DTOs.Documents;

public class DocumentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public Guid UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDocumentDto
{
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }
}

public class UpdateDocumentDto
{
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}

public class DocumentUploadResponseDto
{
    public Guid DocumentId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
