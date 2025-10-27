using Application.DTO.Student.Query;
using Application.DTO.Student;
using Application.DTO.Subject;

namespace Application.Interfaces.Services;

public interface IStudentService
{
    Task AssignSubject(AssignSubjectStudentDto dto);
    Task<List<SubjectResponseDto>> GetStudentSubjects(int studentId);
    Task<IEnumerable<StudentDto>> GetFilteredStudents(StudentFilterDto filter);
}