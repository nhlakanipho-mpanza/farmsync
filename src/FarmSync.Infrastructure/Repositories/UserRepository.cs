using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Auth;
using FarmSync.Application.Interfaces;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FarmSyncDbContext _context;

    public UserRepository(FarmSyncDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<bool> ExistsAsync(string username, string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username || u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }
}
