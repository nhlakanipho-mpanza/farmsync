namespace FarmSync.Infrastructure.Services;

public class SftpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string Username { get; set; } = string.Empty;
    public string PrivateKeyPath { get; set; } = string.Empty;
    public string DocumentsBasePath { get; set; } = string.Empty;
    public string ProfilePicturesBasePath { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
}
