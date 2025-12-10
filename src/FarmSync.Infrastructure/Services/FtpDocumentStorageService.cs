using FarmSync.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FarmSync.Infrastructure.Services;

public class FtpDocumentStorageService : IDocumentStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FtpDocumentStorageService> _logger;

    public FtpDocumentStorageService(IConfiguration configuration, ILogger<FtpDocumentStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string entityType, Guid entityId, string documentType)
    {
        var host = _configuration["FTP:Host"] ?? "smartcubesa.co.za";
        var username = _configuration["FTP:Username"] ?? "smartcu2";
        var password = _configuration["FTP:Password"] ?? "";
        
        // Use PRIVATE path for sensitive documents, PUBLIC for profile pictures
        var isProfilePicture = documentType.Equals("Profile Picture", StringComparison.OrdinalIgnoreCase);
        var basePath = isProfilePicture
            ? _configuration["FTP:PublicBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync"
            : _configuration["FTP:PrivateBasePath"] ?? "/home/smartcu2/assets/farmsync";

        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var extension = Path.GetExtension(fileName);
        var sanitizedFileName = $"{documentType.Replace(" ", "_")}_{timestamp}{extension}";
        var relativePath = $"{entityType.ToLower()}/{entityId}/{sanitizedFileName}";
        var fullPath = $"{basePath}/{relativePath}";

        try
        {
            _logger.LogInformation($"Starting FTP upload to {host}{fullPath}");
            
            // Create directory structure first
            await CreateFtpDirectoryRecursiveAsync(host, username, password, $"{basePath}/{entityType.ToLower()}/{entityId}");

            // Upload file
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{host}{fullPath}");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UseBinary = true;
            request.KeepAlive = false;
            request.UsePassive = true; // Important for firewalls

            using (var requestStream = request.GetRequestStream())
            {
                await fileStream.CopyToAsync(requestStream);
            }

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                _logger.LogInformation($"FTP Upload complete to {(isProfilePicture ? "PUBLIC" : "PRIVATE")} storage: {relativePath}, Status: {response.StatusDescription}");
            }

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload file via FTP: {relativePath}");
            throw new Exception($"FTP upload failed: {ex.Message}", ex);
        }
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        var host = _configuration["FTP:Host"] ?? "smartcubesa.co.za";
        var username = _configuration["FTP:Username"] ?? "smartcu2";
        var password = _configuration["FTP:Password"] ?? "";
        
        // Try private path first, then public
        var privatePath = _configuration["FTP:PrivateBasePath"] ?? "/home/smartcu2/assets/farmsync";
        var publicPath = _configuration["FTP:PublicBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";

        try
        {
            // Try private storage first
            return await DownloadFromFtpPathAsync(host, username, password, $"{privatePath}/{filePath}");
        }
        catch
        {
            // Fallback to public storage
            try
            {
                return await DownloadFromFtpPathAsync(host, username, password, $"{publicPath}/{filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to download file via FTP: {filePath}");
                throw new FileNotFoundException($"File not found in FTP storage: {filePath}", ex);
            }
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        var host = _configuration["FTP:Host"] ?? "smartcubesa.co.za";
        var username = _configuration["FTP:Username"] ?? "smartcu2";
        var password = _configuration["FTP:Password"] ?? "";
        var privatePath = _configuration["FTP:PrivateBasePath"] ?? "/home/smartcu2/assets/farmsync";
        var publicPath = _configuration["FTP:PublicBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";

        try
        {
            // Try private storage first
            return await DeleteFromFtpPathAsync(host, username, password, $"{privatePath}/{filePath}");
        }
        catch
        {
            // Fallback to public storage
            try
            {
                return await DeleteFromFtpPathAsync(host, username, password, $"{publicPath}/{filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete file via FTP: {filePath}");
                return false;
            }
        }
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        var host = _configuration["FTP:Host"] ?? "smartcubesa.co.za";
        var username = _configuration["FTP:Username"] ?? "smartcu2";
        var password = _configuration["FTP:Password"] ?? "";
        var privatePath = _configuration["FTP:PrivateBasePath"] ?? "/home/smartcu2/assets/farmsync";
        var publicPath = _configuration["FTP:PublicBasePath"] ?? "/home/smartcu2/domains/zimeholding.co.za/public_html/assets/farmsync";

        // Check private storage first
        if (await FileExistsAtFtpPathAsync(host, username, password, $"{privatePath}/{filePath}"))
            return true;

        // Check public storage
        return await FileExistsAtFtpPathAsync(host, username, password, $"{publicPath}/{filePath}");
    }

    public string GetPublicUrl(string relativePath)
    {
        // Only profile pictures get public URLs
        var publicUrl = _configuration["FTP:PublicUrl"] ?? "https://zimeholding.co.za/assets/farmsync";
        return $"{publicUrl}/{relativePath.Replace("\\", "/")}";
    }

    // Helper methods
    private async Task<Stream> DownloadFromFtpPathAsync(string host, string username, string password, string fullPath)
    {
        var request = (FtpWebRequest)WebRequest.Create($"ftp://{host}{fullPath}");
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential(username, password);
        request.UseBinary = true;
        request.UsePassive = true;

        using (var response = (FtpWebResponse)await request.GetResponseAsync())
        using (var responseStream = response.GetResponseStream())
        {
            var memoryStream = new MemoryStream();
            await responseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }

    private async Task<bool> DeleteFromFtpPathAsync(string host, string username, string password, string fullPath)
    {
        var request = (FtpWebRequest)WebRequest.Create($"ftp://{host}{fullPath}");
        request.Method = WebRequestMethods.Ftp.DeleteFile;
        request.Credentials = new NetworkCredential(username, password);
        request.UsePassive = true;

        using (var response = (FtpWebResponse)await request.GetResponseAsync())
        {
            _logger.LogInformation($"FTP Delete complete, status: {response.StatusDescription}");
            return true;
        }
    }

    private async Task<bool> FileExistsAtFtpPathAsync(string host, string username, string password, string fullPath)
    {
        try
        {
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{host}{fullPath}");
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;

            using (var response = (FtpWebResponse)await request.GetResponseAsync())
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    private async Task CreateFtpDirectoryRecursiveAsync(string host, string username, string password, string directory)
    {
        var parts = directory.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var currentPath = "";

        foreach (var part in parts)
        {
            currentPath += "/" + part;
            
            try
            {
                var request = (FtpWebRequest)WebRequest.Create($"ftp://{host}{currentPath}");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;

                using (var response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    _logger.LogDebug($"FTP Directory created: {currentPath}");
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is FtpWebResponse response)
                {
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        // Directory already exists, continue
                        continue;
                    }
                }
                // Ignore other errors during directory creation
                _logger.LogDebug($"FTP Directory may already exist: {currentPath}");
            }
        }
    }
}
