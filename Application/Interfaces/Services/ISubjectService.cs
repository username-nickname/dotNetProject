using Application.DTO.Subject;
using Application.DTO.Teacher;

namespace Application.Interfaces.Services;

public interface ISubjectService
{
    Task AddSubject(AddSubjectDto dto);
    Task UpdateSubject(UpdateSubjectDto dto, int subjectId);
    Task<IEnumerable<SubjectResponseDto>> GetAllSubjects();
    Task<IEnumerable<TeacherShortResponseDto>> GetTeachersForSubject(int subjectId);
}