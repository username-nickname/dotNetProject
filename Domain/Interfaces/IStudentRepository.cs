using Domain.Entities;

namespace Domain.Interfaces;

public interface IStudentRepository
{
    Task Add(Student student);
    Task<Student?> GetById(int id);
    Task SaveChangesAsync();
    Task<bool> ExistsById(int id);
    Task<bool> HasSubject(int studentId, int subjectId);

    Task<Student?> GetByIdWithSubjects(int studentId);
}