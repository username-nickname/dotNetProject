using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly AppDbContext _context;

    public SubjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Subject?> GetById(int id)
    {
        return await _context.Subjects.FindAsync(id);
    }
    
    public async Task Add(Subject subject)
    {
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsById(int id)
    {
        return await _context.Subjects.AnyAsync(u => u.Id == id);
    }
    
    public async Task<bool> ExistsByName(string name)
    {
        return await _context.Subjects
            .AnyAsync(r => r.Name == name);
    }

    public async Task<int> CountByDepartment(int departmentId)
    {
        return await _context.Subjects
            .Where(s =>
                s.Teachers.Any(ts => ts.Teacher.DepartmentId == departmentId))
            .CountAsync();
    }
    
    public async Task<IEnumerable<Subject>> GetAll()
    {
        return await _context.Subjects.ToListAsync();
    }
    
    public async Task<IEnumerable<Teacher>> GetAssignedTeachers(int subjectId)
    {
        return await _context.Subjects
            .Where(s => s.Id == subjectId)
            .SelectMany(s => s.Teachers.Select(ts => ts.Teacher))
            .Include(t => t.Position) 
            .Include(t => t.Department)
            .Distinct()
            .ToListAsync();
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}