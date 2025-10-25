using Domain.Entities;

namespace Domain.Interfaces;

public interface IGradeRepository
{
    Task<Grade?> GetById(int id);
    Task<bool> ExistsById(int id);
    Task AddAsync(Grade grade); 
    Task DeleteAsync(Grade grade);
    Task SaveChangesAsync();
    Task<List<Grade>> GetGradesByStudentAndSubject(int studentId, int subjectId);
    Task<List<Grade>> GetGradesByStudentAndSemester(int studentId, int semester);
    Task<List<Grade>> GetGradesByStudentId(int studentId);
    Task<List<Grade>> GetGradesByGroupAndSemester(string groupName, int semester);
    Task<IEnumerable<Grade>> GetGradesByTeacher(int teacherId);
    Task<IEnumerable<Grade>> GetGradesByDepartment(int departmentId);
}