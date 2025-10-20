namespace Infrastructure.Persistence.Repositories;

using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    
    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsByName(string name)
    {
        return await _context.Roles
            .AnyAsync(r => r.Name == name);
    }
}