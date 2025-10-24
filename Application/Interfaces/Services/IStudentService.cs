namespace Application.Interfaces.Services;

using DTO.Student;
using DTO.Subject;

public interface IStudentService
{
    Task AssignSubject(AssignSubjectStudentDto dto);
    Task<List<SubjectResponseDto>> GetStudentSubjects(int studentId);
}