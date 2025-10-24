using Application.DTO.Grade;
using Application.DTO.Grade.Response;
using Application.DTO.Grade.Query;

namespace Application.Interfaces.Services;

public interface IGradeService
{
    Task AddGrade(AddGradeDto dto, int userId);
    Task UpdateGrade(UpdateGradeDto dto, int userId);
    Task DeleteGrade(int gradeId, int userId);
    Task<List<GradeResponseDto>> GetGradesForSubject(GetGradesQueryDto queryDto);
    Task<StudentGpaResponseDto> CalculateStudentGpa(CalculateStudentGpaQueryDto dto);
}