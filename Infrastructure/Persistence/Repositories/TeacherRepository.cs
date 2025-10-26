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
    
    public async Task<Teacher?> GetById(int id)
    {
        return await _context.Teachers.FindAsync(id);
    }
    
    public async Task<Teacher?> GetByIdWithSubjects(int id)
    {
        return await _context.Teachers
            .Include(t => t.Subjects) 
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<bool> ExistsById(int id)
    {
        return await _context.Teachers.AnyAsync(u => u.Id == id);
    }
    
    public async Task<bool> HasSubject(int teacherId, int subjectId)
    {
        return await _context.TeacherSubjects.AnyAsync(ss =>
            ss.TeacherId == teacherId && ss.SubjectId == subjectId);
    }
    
    public async Task<int> CountByDepartment(int departmentId)
    {
        return await _context.Teachers
            .CountAsync(t => t.DepartmentId == departmentId);
    }
    
    public async Task<IEnumerable<Subject>> GetAssignedSubjects(int teacherId)
    {
        var subjects = await _context.Teachers
            .Where(t => t.Id == teacherId)
            .SelectMany(t => t.Subjects.Select(ts => ts.Subject))
            .Distinct()
            .ToListAsync();
        
        return subjects;
    }
    
    public async Task<IEnumerable<Teacher>> SearchByName(string nameQuery)
    {
        var query = nameQuery.Trim().ToLower();

        return await _context.Teachers
            .Where(t => t.FullName.ToLower().StartsWith(query)) 
            .Include(t => t.Position)
            .Include(t => t.Department)
            .ToListAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}