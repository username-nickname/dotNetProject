using Application.DTO.Subject;
using Application.DTO.Teacher;
using Application.Interfaces.Services;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IValidator<AssignSubjectTeacherDto> _assignSubjectTeacherValidator;
    private readonly IValidator<UnassignSubjectTeacherDto> _unassignSubjectTeacherValidator;

    public TeacherService(
        ITeacherRepository teacherRepository,
        IValidator<AssignSubjectTeacherDto> assignSubjectTeacherValidator,
        IValidator<UnassignSubjectTeacherDto> unassignSubjectTeacherValidator
    )
    {
        _teacherRepository = teacherRepository;
        _assignSubjectTeacherValidator = assignSubjectTeacherValidator;
        _unassignSubjectTeacherValidator = unassignSubjectTeacherValidator;
    }
    
    public async Task AssignSubject(AssignSubjectTeacherDto dto)
    {
        await _assignSubjectTeacherValidator.ValidateAndThrowAsync(dto);
        
        var teacher = await _teacherRepository.GetByIdWithSubjects(dto.TeacherId);
        if (teacher == null) throw new UserNotFoundException($"Teacher with ID {dto.TeacherId} not found");

        teacher.AssignSubject(dto.SubjectId); 
        
        await _teacherRepository.SaveChangesAsync();
    }

    public async Task UnassignSubject(UnassignSubjectTeacherDto dto)
    {
        await _unassignSubjectTeacherValidator.ValidateAndThrowAsync(dto);
        
        var teacher = await _teacherRepository.GetByIdWithSubjects(dto.TeacherId);
        if (teacher == null) throw new UserNotFoundException($"Teacher with ID {dto.TeacherId} not found");

        teacher.UnassignSubject(dto.SubjectId); 
        
        await _teacherRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubjectResponseDto>> GetAssignedSubjects(int teacherId)
    {
        if (!await _teacherRepository.ExistsById(teacherId)) 
            throw new UserNotFoundException($"Teacher with ID {teacherId} not found.");
            
        var subjects = await _teacherRepository.GetAssignedSubjects(teacherId);

        return subjects.Select(s => new SubjectResponseDto(s.Id, s.Name,s.Semester, s.Credits));
    }
    
    public async Task<IEnumerable<TeacherShortResponseDto>> SearchTeachersByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return [];
        }
        
        var teachers = await _teacherRepository.SearchByName(name);

        return teachers.Select(teacher => new TeacherShortResponseDto
        {
            Id = teacher.Id,
            FullName = teacher.FullName,
            PositionName = teacher.Position?.Name ?? "N/A",
            DepartmentName = teacher.Department?.Name ?? "N/A"
        });
    }
}