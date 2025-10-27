using Domain.Entities;

namespace Domain.Interfaces;

public interface IStudentRepository
{
    Task Add(Student student);
    Task<Student?> GetById(int id);
    // TODO: ПРоверить ,используется ли где-то этот метод 
    Task<Student?> GetStudentWithDetails(int id);
    Task<IEnumerable<Student>> GetByGroup(string groupName);
    Task SaveChangesAsync();
    Task<bool> ExistsById(int id);
    Task<bool> GroupExistsByName(string groupName);
    Task<bool> HasSubject(int studentId, int subjectId);
    Task<Student?> GetByIdWithSubjects(int studentId);
    Task<int> CountByDepartment(int departmentId);
    IQueryable<Student> GetAsQueryable();
}