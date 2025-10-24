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

    public async Task<bool> ExistsById(int id)
    {
        return await _context.Students.AnyAsync(u => u.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasSubject(int studentId, int subjectId)
    {
        return await _context.StudentSubjects.AnyAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);
    }

    public async Task<Student?> GetByIdWithSubjects(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Subjects)
            .ThenInclude(ss => ss.Subject)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        return student;
    }
}