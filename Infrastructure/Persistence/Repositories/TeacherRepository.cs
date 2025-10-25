using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(Teacher teacher)
    {
        await _context.Teachers.AddAsync(teacher);
    }

    public async Task<Teacher?> GetByUserId(int id)
    {
        return await _context.Teachers
            .Where(t => t.UserId == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> ExistsById(int id)
    {
        return await _context.Teachers.AnyAsync(u => u.Id == id);
    }
    
    public async Task<int> CountByDepartment(int departmentId)
    {
        return await _context.Teachers
            .CountAsync(t => t.DepartmentId == departmentId);
    }
}