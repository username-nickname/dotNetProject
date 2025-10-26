using Application.DTO.Subject;
using Application.DTO.Teacher;

namespace Application.Interfaces.Services;

public interface ITeacherService
{
    Task AssignSubject(AssignSubjectTeacherDto dto);
    Task UnassignSubject(UnassignSubjectTeacherDto dto);
    Task<IEnumerable<SubjectResponseDto>> GetAssignedSubjects(int teacherId);
    Task<IEnumerable<TeacherShortResponseDto>> SearchTeachersByName(string name);
}