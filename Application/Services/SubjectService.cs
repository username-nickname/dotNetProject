using Application.DTO.Subject;
using Application.DTO.Teacher;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class SubjectService : ISubjectService
{
    private readonly IValidator<AddSubjectDto> _addSubjectValidator;
    private readonly IValidator<UpdateSubjectDto> _updateSubjectValidator;
    private readonly ISubjectRepository _subjectRepository;
    
    public SubjectService(
        IValidator<AddSubjectDto> addSubjectValidator,
        IValidator<UpdateSubjectDto> updateSubjectValidator,
        ISubjectRepository subjectRepository
    )
    {
        _addSubjectValidator = addSubjectValidator;
        _updateSubjectValidator = updateSubjectValidator;
        _subjectRepository = subjectRepository;
    }

    
    public async Task AddSubject(AddSubjectDto dto)
    {
        await _addSubjectValidator.ValidateAndThrowAsync(dto);

        var subject = Subject.CreateNew(dto.Name, dto.Semester, dto.Credits);
        
        await _subjectRepository.Add(subject);
    }

    public async Task UpdateSubject(UpdateSubjectDto dto, int subjectId)
    {
        await _updateSubjectValidator.ValidateAndThrowAsync(dto);

        var subject = await _subjectRepository.GetById(subjectId);

        if (subject == null) throw new SubjectNotFoundException(subjectId);

        if (!string.IsNullOrEmpty(dto.Name))
        {
            subject.UpdateName(dto.Name); 
        }
        
        if (dto.Semester.HasValue)
        {
            subject.UpdateSemester(dto.Semester.Value);
        }

        if (dto.Credits.HasValue)
        {
            subject.UpdateCredits(dto.Credits.Value);
        }

        await _subjectRepository.SaveChanges();
    }

    public async Task<IEnumerable<SubjectResponseDto>> GetAllSubjects()
    {
        var subjects = await _subjectRepository.GetAll();
        
        var result = subjects.Select(subject => new SubjectResponseDto(
            subject.Id, 
            subject.Name,
            subject.Semester,
            subject.Credits
            ));

        return result;
    }
    
    public async Task<IEnumerable<TeacherShortResponseDto>> GetTeachersForSubject(int subjectId)
    {
        if (!await _subjectRepository.ExistsById(subjectId)) throw new SubjectNotFoundException(subjectId);

        var teachers = await _subjectRepository.GetAssignedTeachers(subjectId);

        var result = teachers.Select(teacher => new TeacherShortResponseDto
        {
            Id = teacher.Id,
            FullName = teacher.FullName,
            PositionName = teacher.Position?.Name ?? "N/A",
            DepartmentName = teacher.Department?.Name ?? "N/A"
        });

        return result;
    }
}