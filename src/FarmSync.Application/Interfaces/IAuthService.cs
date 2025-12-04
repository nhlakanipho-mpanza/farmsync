using FarmSync.Application.DTOs.Auth;

namespace FarmSync.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<bool> RegisterAsync(RegisterUserDto request);
    string GenerateJwtToken(Guid userId, string username, List<string> roles);
}
