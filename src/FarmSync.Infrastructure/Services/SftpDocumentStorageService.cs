using FarmSync.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace FarmSync.Infrastructure.Services;

public class SftpDocumentStorageService : IDocumentStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SftpDocumentStorageService> _logger;

    public SftpDocumentStorageService(IConfiguration configuration, ILogger<SftpDocumentStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private SftpClient GetSftpClient()
    {
        var host = _configuration["SFTP:Host"] ?? "zimeholding.co.za";
        var port = int.Parse(_configuration["SFTP:Port"] ?? "22");
        var username = _configuration["SFTP:Username"] ?? "smartcu2";
        var keyFilePath = _configuration["SFTP:PrivateKeyPath"];
        var password = _configuration["SFTP:Password"];

        // Try SSH key authentication first, fall back to password
        if (!string.IsNullOrEmpty(keyFilePath) && File.Exists(keyFilePath))
        {
            _logger.LogInformation("Using SSH key authentication for SFTP");
            var keyFile = new PrivateKeyFile(keyFilePath);
            var connectionInfo = new ConnectionInfo(
                host,
                port,
                username,
                new PrivateKeyAuthenticationMethod(username, keyFile)
            );
            return new SftpClient(connectionInfo);
        }
        else if (!string.IsNullOrEmpty(password))
        {
            _logger.LogInformation("Using password authentication for SFTP");
            return new SftpClient(host, port, username, password);
        }
        else
        {
            throw new InvalidOperationException(
                "SFTP authentication failed: No valid SSH key or password configured. " +
                $"Key path: {keyFilePath ?? "not set"}, Password: {(string.IsNullOrEmpty(password) ? "not set" : "set")}"
            );
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string entityType, Guid entityId, string documentType)
    {
        try
        {
            // Determine base path based on document type
            var isProfilePicture = documentType.Equals("Profile Picture", StringComparison.OrdinalIgnoreCase);
            var basePath = isProfilePicture 
                ? _configuration["SFTP:ProfilePicturesBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync"
                : _configuration["SFTP:DocumentsBasePath"] ?? "/home/smartcu2/assets/farmsync";

            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var extension = Path.GetExtension(fileName);
            var sanitizedFileName = $"{documentType}_{timestamp}{extension}";
            var relativePath = $"{entityType.ToLower()}/{entityId}/{sanitizedFileName}";
            var fullPath = $"{basePath}/{relativePath}";

            using var client = GetSftpClient();
            client.Connect();

            // Create directory if it doesn't exist
            var directory = Path.GetDirectoryName(fullPath)!.Replace("\\", "/");
            if (!client.Exists(directory))
            {
                CreateDirectoryRecursive(client, directory);
            }

            // Upload file
            await Task.Run(() => client.UploadFile(fileStream, fullPath));

            client.Disconnect();
            _logger.LogInformation($"File uploaded successfully: {relativePath}");

            return relativePath;
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
            // Determine base path (try profile pictures first, then documents)
            var profilePicturesBasePath = _configuration["SFTP:ProfilePicturesBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";
            var documentsBasePath = _configuration["SFTP:DocumentsBasePath"] ?? "/home/smartcu2/assets/farmsync";
            
            string fullPath;
            using var client = GetSftpClient();
            client.Connect();

            // Try profile pictures path first
            fullPath = $"{profilePicturesBasePath}/{filePath}";
            if (!client.Exists(fullPath))
            {
                // Try documents path
                fullPath = $"{documentsBasePath}/{filePath}";
            }

            var memoryStream = new MemoryStream();
            await Task.Run(() => client.DownloadFile(fullPath, memoryStream));

            client.Disconnect();
            memoryStream.Position = 0;

            _logger.LogInformation($"File downloaded successfully: {filePath}");
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to download file: {filePath}");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            // Determine base path (try profile pictures first, then documents)
            var profilePicturesBasePath = _configuration["SFTP:ProfilePicturesBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";
            var documentsBasePath = _configuration["SFTP:DocumentsBasePath"] ?? "/home/smartcu2/assets/farmsync";
            
            using var client = GetSftpClient();
            client.Connect();

            // Try profile pictures path first
            var fullPath = $"{profilePicturesBasePath}/{filePath}";
            if (!client.Exists(fullPath))
            {
                // Try documents path
                fullPath = $"{documentsBasePath}/{filePath}";
            }

            if (client.Exists(fullPath))
            {
                await Task.Run(() => client.DeleteFile(fullPath));
                client.Disconnect();
                _logger.LogInformation($"File deleted successfully: {filePath}");
                return true;
            }

            client.Disconnect();
            _logger.LogWarning($"File not found for deletion: {filePath}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete file: {filePath}");
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        try
        {
            // Determine base path (try profile pictures first, then documents)
            var profilePicturesBasePath = _configuration["SFTP:ProfilePicturesBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";
            var documentsBasePath = _configuration["SFTP:DocumentsBasePath"] ?? "/home/smartcu2/assets/farmsync";
            
            using var client = GetSftpClient();
            client.Connect();

            // Try profile pictures path first
            var fullPath = $"{profilePicturesBasePath}/{filePath}";
            var exists = await Task.Run(() => client.Exists(fullPath));
            
            if (!exists)
            {
                // Try documents path
                fullPath = $"{documentsBasePath}/{filePath}";
                exists = await Task.Run(() => client.Exists(fullPath));
            }

            client.Disconnect();
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to check file existence: {filePath}");
            return false;
        }
    }

    public string GetPublicUrl(string relativePath)
    {
        var baseUrl = _configuration["SFTP:PublicUrl"] ?? "https://zimeholding.co.za/assets/farmsync";
        return $"{baseUrl}/{relativePath}";
    }

    private void CreateDirectoryRecursive(SftpClient client, string path)
    {
        var parts = path.Split('/');
        var currentPath = "";

        foreach (var part in parts)
        {
            if (string.IsNullOrWhiteSpace(part))
                continue;

            currentPath += "/" + part;

            if (!client.Exists(currentPath))
            {
                client.CreateDirectory(currentPath);
                _logger.LogInformation($"Created directory: {currentPath}");
            }
        }
    }
}
