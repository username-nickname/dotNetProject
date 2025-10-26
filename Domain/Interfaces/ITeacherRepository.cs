namespace Domain.Interfaces;

using Entities;

public interface ITeacherRepository
{
    Task Add(Teacher teacher);
    Task<bool> ExistsById(int id);
    Task<Teacher?> GetByUserId(int id);
    Task<Teacher?> GetById(int id);
    Task<Teacher?> GetByIdWithSubjects(int id);
    Task<bool> HasSubject(int teacherId, int subjectId);
    Task<int> CountByDepartment(int departmentId);
    Task<IEnumerable<Subject>> GetAssignedSubjects(int teacherId);
    Task<IEnumerable<Teacher>> SearchByName(string nameQuery);
    Task SaveChangesAsync();
}