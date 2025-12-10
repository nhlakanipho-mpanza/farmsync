using FarmSync.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FarmSync.Infrastructure.Services;

public class LocalFileStorageService : IDocumentStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly string _basePath;
    private readonly string _publicUrlBase;

    public LocalFileStorageService(IConfiguration configuration, ILogger<LocalFileStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        // Use a local directory for file storage
        _basePath = _configuration["FileStorage:LocalPath"] ?? 
                   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _publicUrlBase = _configuration["FileStorage:PublicUrl"] ?? "http://localhost:5201/uploads";
        
        // Ensure base directory exists
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
            _logger.LogInformation($"Created local storage directory: {_basePath}");
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string entityType, Guid entityId, string documentType)
    {
        try
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var extension = Path.GetExtension(fileName);
            var sanitizedFileName = $"{documentType.Replace(" ", "_")}_{timestamp}{extension}";
            var relativePath = Path.Combine(entityType.ToLower(), entityId.ToString(), sanitizedFileName);
            var fullPath = Path.Combine(_basePath, relativePath);

            // Create directory if it doesn't exist
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            // Save file
            using (var fileStreamOut = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileStreamOut);
            }

            _logger.LogInformation($"File uploaded successfully to local storage: {fullPath}");
            return relativePath.Replace("\\", "/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload file for {entityType}/{entityId}");
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, filePath);
            
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to download file: {filePath}");
            throw;
        }
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, filePath);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation($"File deleted from local storage: {filePath}");
                return Task.FromResult(true);
            }
            
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete file: {filePath}");
            return Task.FromResult(false);
        }
    }

    public Task<bool> FileExistsAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        return Task.FromResult(File.Exists(fullPath));
    }

    public string GetPublicUrl(string relativePath)
    {
        return $"{_publicUrlBase}/{relativePath.Replace("\\", "/")}";
    }
}
