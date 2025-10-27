using Application.DTO.Student.Query;

namespace Application.Interfaces.Services;

using DTO.Student;
using DTO.Subject;

public interface IStudentService
{
    Task AssignSubject(AssignSubjectStudentDto dto);
    Task<List<SubjectResponseDto>> GetStudentSubjects(int studentId);
    Task<IEnumerable<StudentShortResponseDto>> GetStudentsByGroup(string groupName);
    Task<IEnumerable<StudentDto>> GetFilteredStudents(StudentFilterDto filter);
}