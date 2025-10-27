using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Student student)
    {
        await _context.Students.AddAsync(student);
    }

    public async Task<Student?> GetById(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<Student?> GetStudentWithDetails(int id)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.Subjects)
            .ThenInclude(ss => ss.Subject)
            .ThenInclude(sub => sub.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetByGroup(
        string groupName)
    {
        return await _context.Students
            .Where(s => s.Group == groupName)
            .ToListAsync();
    }

    public async Task<bool> ExistsById(int id)
    {
        return await _context.Students.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> GroupExistsByName(string groupName)
    {
        return await _context.Students.AnyAsync(s => s.Group == groupName);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasSubject(int studentId, int subjectId)
    {
        return await _context.StudentSubjects.AnyAsync(ss =>
            ss.StudentId == studentId && ss.SubjectId == subjectId);
    }

    public async Task<Student?> GetByIdWithSubjects(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Subjects)
            .ThenInclude(ss => ss.Subject)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        return student;
    }

    public async Task<int> CountByDepartment(int departmentId)
    {
        return await _context.Students
            .Where(s => _context.Grades.Any(g =>
                g.StudentId == s.Id &&
                g.Teacher.DepartmentId == departmentId))
            .CountAsync();
    }

    public IQueryable<Student> GetAsQueryable()
    {
        return _context.Students
            .Include(s => s.Subjects)
            .ThenInclude(ss => ss.Subject)
            .AsQueryable();
    }
}