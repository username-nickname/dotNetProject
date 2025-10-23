namespace Infrastructure.Persistence.Repositories;

using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PositionRepository : IPositionRepository
{
    private readonly AppDbContext _context;
    
    public PositionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsById(int id)
    {
        return await _context.Positions.AnyAsync(u => u.Id == id);
    }
}