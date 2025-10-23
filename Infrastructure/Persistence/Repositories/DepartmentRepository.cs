using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;
    
    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Department> GetById(int id)
    {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<bool> ExistsById(int id)
    {
        return await _context.Departments.AnyAsync(u => u.Id == id);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}