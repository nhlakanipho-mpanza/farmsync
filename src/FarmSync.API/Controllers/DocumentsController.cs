using FarmSync.Application.DTOs.Documents;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Documents;
using FarmSync.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly FarmSyncDbContext _context;
    private readonly IDocumentStorageService _storageService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(
        FarmSyncDbContext context,
        IDocumentStorageService storageService,
        ILogger<DocumentsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<DocumentUploadResponseDto>> UploadDocument(
        [FromForm] IFormFile file,
        [FromForm] string entityType,
        [FromForm] Guid entityId,
        [FromForm] Guid documentTypeId,
        [FromForm] DateTime? expiryDate = null,
        [FromForm] string? notes = null)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Validate file size (e.g., max 10MB)
            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File size exceeds 10MB limit");

            // Validate document type exists
            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(dt => dt.Id == documentTypeId && dt.IsActive);

            if (documentType == null)
                return BadRequest("Invalid document type");

            // Get user ID from claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
                ?? User.FindFirst("sub") 
                ?? User.FindFirst("userId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("User ID not found in token. Available claims: {Claims}", 
                    string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}")));
                return Unauthorized("User ID not found in token");
            }

            // Upload file to SFTP
            string relativePath;
            using (var stream = file.OpenReadStream())
            {
                relativePath = await _storageService.UploadFileAsync(
                    stream,
                    file.FileName,
                    entityType,
                    entityId,
                    documentType.Name);
            }

            // Get public URL (only for profile pictures)
            var isProfilePicture = documentType.Name.Equals("Profile Picture", StringComparison.OrdinalIgnoreCase);
            var publicUrl = isProfilePicture ? _storageService.GetPublicUrl(relativePath) : string.Empty;

            // Create document record
            var document = new Document
            {
                Id = Guid.NewGuid(),
                FileName = Path.GetFileName(relativePath),
                OriginalFileName = file.FileName,
                FilePath = relativePath,
                FileUrl = publicUrl,
                FileSize = file.Length,
                MimeType = file.ContentType,
                EntityType = entityType,
                EntityId = entityId,
                DocumentTypeId = documentTypeId,
                ExpiryDate = expiryDate,
                UploadedBy = userId,
                UploadedAt = DateTime.UtcNow,
                Notes = notes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Document uploaded: {file.FileName} for {entityType}/{entityId}");

            return Ok(new DocumentUploadResponseDto
            {
                DocumentId = document.Id,
                FileUrl = publicUrl,
                Message = "Document uploaded successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document");
            return StatusCode(500, "An error occurred while uploading the document");
        }
    }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<List<DocumentDto>>> GetEntityDocuments(string entityType, Guid entityId)
    {
        var documents = await _context.Documents
            .Include(d => d.DocumentType)
            .Where(d => d.EntityType == entityType && d.EntityId == entityId && d.IsActive)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                OriginalFileName = d.OriginalFileName,
                FileUrl = d.FileUrl,
                FileSize = d.FileSize,
                MimeType = d.MimeType,
                EntityType = d.EntityType,
                EntityId = d.EntityId,
                DocumentType = d.DocumentType.Name,
                ExpiryDate = d.ExpiryDate,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Notes = d.Notes,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();

        return Ok(documents);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DocumentDto>> GetDocument(Guid id)
    {
        var document = await _context.Documents
            .Include(d => d.DocumentType)
            .Where(d => d.Id == id)
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                OriginalFileName = d.OriginalFileName,
                FileUrl = d.FileUrl,
                FileSize = d.FileSize,
                MimeType = d.MimeType,
                EntityType = d.EntityType,
                EntityId = d.EntityId,
                DocumentType = d.DocumentType.Name,
                ExpiryDate = d.ExpiryDate,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Notes = d.Notes,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (document == null)
            return NotFound();

        return Ok(document);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null || !document.IsActive)
            return NotFound();

        try
        {
            var fileStream = await _storageService.DownloadFileAsync(document.FilePath);
            return File(fileStream, document.MimeType, document.OriginalFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading document {id}");
            return StatusCode(500, "An error occurred while downloading the document");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] UpdateDocumentDto dto)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
            return NotFound();

        document.ExpiryDate = dto.ExpiryDate;
        document.Notes = dto.Notes;
        document.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
            return NotFound();

        try
        {
            // Soft delete
            document.IsActive = false;
            await _context.SaveChangesAsync();

            // Optionally delete from SFTP (hard delete)
            // await _storageService.DeleteFileAsync(document.FilePath);

            _logger.LogInformation($"Document deleted: {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting document {id}");
            return StatusCode(500, "An error occurred while deleting the document");
        }
    }

    [HttpGet("expiring")]
    public async Task<ActionResult<List<DocumentDto>>> GetExpiringDocuments([FromQuery] int daysAhead = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);

        var documents = await _context.Documents
            .Include(d => d.DocumentType)
            .Where(d => d.IsActive &&
                       d.ExpiryDate.HasValue &&
                       d.ExpiryDate.Value <= cutoffDate &&
                       d.ExpiryDate.Value >= DateTime.UtcNow)
            .OrderBy(d => d.ExpiryDate)
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                OriginalFileName = d.OriginalFileName,
                FileUrl = d.FileUrl,
                FileSize = d.FileSize,
                MimeType = d.MimeType,
                EntityType = d.EntityType,
                EntityId = d.EntityId,
                DocumentType = d.DocumentType.Name,
                ExpiryDate = d.ExpiryDate,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Notes = d.Notes,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();

        return Ok(documents);
    }

    [HttpGet("expired")]
    public async Task<ActionResult<List<DocumentDto>>> GetExpiredDocuments()
    {
        var documents = await _context.Documents
            .Include(d => d.DocumentType)
            .Where(d => d.IsActive &&
                       d.ExpiryDate.HasValue &&
                       d.ExpiryDate.Value < DateTime.UtcNow)
            .OrderBy(d => d.ExpiryDate)
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                OriginalFileName = d.OriginalFileName,
                FileUrl = d.FileUrl,
                FileSize = d.FileSize,
                MimeType = d.MimeType,
                EntityType = d.EntityType,
                EntityId = d.EntityId,
                DocumentType = d.DocumentType.Name,
                ExpiryDate = d.ExpiryDate,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Notes = d.Notes,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();

        return Ok(documents);
    }
}
