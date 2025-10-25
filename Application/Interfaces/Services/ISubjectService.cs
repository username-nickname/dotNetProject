using Application.DTO.Subject;

namespace Application.Interfaces.Services;

public interface ISubjectService
{
    Task AddSubject(AddSubjectDto dto);
    Task UpdateSubject(UpdateSubjectDto dto, int subjectId);
    Task<IEnumerable<SubjectResponseDto>> GetAllSubjects();
}