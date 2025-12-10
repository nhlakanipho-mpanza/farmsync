using Microsoft.Extensions.Options;
using Renci.SshNet;
using System.Text;

namespace FarmSync.Infrastructure.Services;

public interface ISftpService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string subfolder = "", bool isProfilePicture = false);
    Task<Stream> DownloadFileAsync(string filePath);
    Task<bool> DeleteFileAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);
    string GetPublicUrl(string fileName, string subfolder = "", bool isProfilePicture = false);
}

public class SftpService : ISftpService, IDisposable
{
    private readonly SftpSettings _settings;
    private SftpClient? _client;
    private readonly object _lock = new object();

    public SftpService(IOptions<SftpSettings> settings)
    {
        _settings = settings.Value;
    }

    private SftpClient GetClient()
    {
        if (_client != null && _client.IsConnected)
        {
            return _client;
        }

        lock (_lock)
        {
            if (_client != null && _client.IsConnected)
            {
                return _client;
            }

            try
            {
                var privateKeyFile = new PrivateKeyFile(_settings.PrivateKeyPath);
                var connectionInfo = new ConnectionInfo(
                    _settings.Host,
                    _settings.Port,
                    _settings.Username,
                    new PrivateKeyAuthenticationMethod(_settings.Username, privateKeyFile)
                );

                _client = new SftpClient(connectionInfo);
                _client.Connect();

                return _client;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to connect to SFTP server: {ex.Message}", ex);
            }
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string subfolder = "", bool isProfilePicture = false)
    {
        try
        {
            var client = GetClient();
            
            // Determine base path
            var basePath = isProfilePicture ? _settings.ProfilePicturesBasePath : _settings.DocumentsBasePath;
            
            // Build full remote path
            var remotePath = string.IsNullOrEmpty(subfolder) 
                ? basePath 
                : $"{basePath}/{subfolder}";

            // Ensure directory exists
            await Task.Run(() => EnsureDirectoryExists(client, remotePath));

            // Generate unique filename to avoid conflicts
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
            var fullPath = $"{remotePath}/{uniqueFileName}";

            // Upload file
            await Task.Run(() => client.UploadFile(fileStream, fullPath, true));

            return uniqueFileName;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload file to SFTP: {ex.Message}", ex);
        }
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        try
        {
            var client = GetClient();
            var memoryStream = new MemoryStream();
            
            await Task.Run(() => client.DownloadFile(filePath, memoryStream));
            memoryStream.Position = 0;
            
            return memoryStream;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to download file from SFTP: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var client = GetClient();
            
            if (!client.Exists(filePath))
            {
                return false;
            }

            await Task.Run(() => client.DeleteFile(filePath));
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file from SFTP: {ex.Message}", ex);
        }
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        try
        {
            var client = GetClient();
            return await Task.Run(() => client.Exists(filePath));
        }
        catch
        {
            return false;
        }
    }

    public string GetPublicUrl(string fileName, string subfolder = "", bool isProfilePicture = false)
    {
        // Profile pictures are publicly accessible, documents are not
        if (!isProfilePicture)
        {
            return string.Empty; // Documents require authenticated download
        }

        var path = string.IsNullOrEmpty(subfolder)
            ? fileName
            : $"{subfolder}/{fileName}";

        return $"{_settings.PublicUrl}/{path}";
    }

    private void EnsureDirectoryExists(SftpClient client, string path)
    {
        var parts = path.Split('/').Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        var currentPath = new StringBuilder();

        foreach (var part in parts)
        {
            currentPath.Append($"/{part}");
            var pathToCheck = currentPath.ToString();

            if (!client.Exists(pathToCheck))
            {
                client.CreateDirectory(pathToCheck);
            }
        }
    }

    public void Dispose()
    {
        if (_client != null)
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
            _client.Dispose();
            _client = null;
        }
    }
}
