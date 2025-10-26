using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly AppDbContext _context;

    public GradeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Grade?> GetById(int id)
    {
        return await _context.Grades.FindAsync(id);
    }

    public async Task<bool> ExistsById(int id)
    {
        return await _context.Grades.AnyAsync(x => x.Id == id);
    }

    public async Task AddAsync(Grade grade)
    {
        await _context.Grades.AddAsync(grade);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Grade grade)
    {
        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Grade>> GetGradesByStudentAndSubject(int studentId,
        int subjectId)
    {
        return await _context.Grades
            .Where(g => g.StudentId == studentId && g.SubjectId == subjectId)
            .ToListAsync();
    }

    public async Task<List<Grade>> GetGradesByStudentAndSemester(int studentId,
        int semester)
    {
        return await _context.Grades
            .Include(g => g.Subject)
            .Where(g => g.StudentId == studentId)
            .Where(g => g.Subject.Semester == semester)
            .ToListAsync();
    }

    public async Task<List<Grade>> GetGradesByStudentId(int studentId)
    {
        return await _context.Grades
            .Include(g => g.Subject)
            .Include(g => g.Teacher)
            .Where(g => g.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<List<Grade>> GetGradesByGroupAndSemester(string groupName,
        int semester)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .Where(g =>
                g.Student.Group == groupName && g.Subject.Semester == semester)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetGradesByTeacher(int teacherId)
    {
        return await _context.Grades
            .Include(g => g.Subject)
            .Where(g => g.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetGradesByGroupAsync(string groupName)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Where(g => g.Student.Group == groupName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetGradesByDepartment(
        int departmentId)
    {
        return await _context.Grades
            .Include(g => g.Teacher)
            .Include(g => g.Student)
            .Where(g => g.Teacher.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}