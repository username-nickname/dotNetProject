namespace Infrastructure.Persistence.Repositories;

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<int> GetTokenVersion(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.TokenVersion)
            .FirstOrDefaultAsync();
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
    
}