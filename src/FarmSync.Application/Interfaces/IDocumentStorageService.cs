namespace FarmSync.Application.Interfaces;

public interface IDocumentStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string entityType, Guid entityId, string documentType);
    Task<Stream> DownloadFileAsync(string filePath);
    Task<bool> DeleteFileAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);
    string GetPublicUrl(string relativePath);
}
