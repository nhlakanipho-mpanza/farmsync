using FarmSync.Domain.Entities.Auth;

namespace FarmSync.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string username, string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
