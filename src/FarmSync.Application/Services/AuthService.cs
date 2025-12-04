using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FarmSync.Application.DTOs.Auth;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
            return null;

        // Verify password (in production, use BCrypt or similar)
        if (!VerifyPassword(request.Password, user.PasswordHash))
            return null;

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var token = GenerateJwtToken(user.Id, user.Username, roles);
        var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow.AddHours(2);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddHours(2).AddMinutes(expiryMinutes)
        };
    }

    public async Task<bool> RegisterAsync(RegisterUserDto request)
    {
        // Check if user already exists
        if (await _userRepository.ExistsAsync(request.Username, request.Email))
            return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public string GenerateJwtToken(Guid userId, string username, List<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2).AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        // In production, use BCrypt.Net or similar
        // This is a simplified example
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        // In production, use BCrypt.Net or similar
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
