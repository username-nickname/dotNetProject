using Domain.Entities;

namespace Domain.Interfaces;

public interface IStudentRepository
{
    Task Add(Student student);
    Task<Student?> GetById(int id);
    Task<Student?> GetStudentWithDetails(int id);
    Task<IEnumerable<Student>> GetByGroup(string groupName);
    Task SaveChangesAsync();
    Task<bool> ExistsById(int id);
    Task<bool> GroupExistsByName(string groupName);
    Task<bool> HasSubject(int studentId, int subjectId);
    Task<Student?> GetByIdWithSubjects(int studentId);
    Task<int> CountByDepartment(int departmentId);
}