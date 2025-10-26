using Application.DTO.Grade;
using Application.DTO.Grade.Response;
using Application.DTO.Grade.Query;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class GradeService : IGradeService
{
    private readonly IGradeConverter _gradeConverter;
    private readonly IValidator<AddGradeDto> _addGradeValidator;
    private readonly IValidator<UpdateGradeDto> _updateGradeValidator;
    private readonly IValidator<GetGradesQueryDto> _getGradeValidator;
    private readonly IValidator<CalculateStudentGpaQueryDto> _calculateStudentGpaQueryValidator;
    private readonly IGradeRepository _gradeRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IStudentRepository _studentRepository;

    public GradeService(
        IGradeConverter gradeConverter,
        IGradeRepository gradeRepository,
        ITeacherRepository teacherRepository,
        IStudentRepository studentRepository,
        IValidator<AddGradeDto> addGradeValidator,
        IValidator<UpdateGradeDto> updateGradeValidator,
        IValidator<GetGradesQueryDto> getGradeValidator,
        IValidator<CalculateStudentGpaQueryDto> calculateStudentGpaQueryValidator
        )
    {
        _gradeConverter = gradeConverter;
        _gradeRepository = gradeRepository;
        _teacherRepository = teacherRepository;
        _addGradeValidator = addGradeValidator;
        _updateGradeValidator = updateGradeValidator;
        _getGradeValidator = getGradeValidator;
        _studentRepository = studentRepository;
        _calculateStudentGpaQueryValidator = calculateStudentGpaQueryValidator;
    }
    
    public async Task AddGrade(AddGradeDto dto, int userId)
    {
        var validationResult = await _addGradeValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }

        var teacher = await _teacherRepository.GetByUserId(userId);
        
        if (teacher == null) throw new UserNotFoundException("Вчителя не знайдено");
        
        var teachesSubject = await _teacherRepository.HasSubject(teacher.Id, dto.SubjectId);
        
        if (!teachesSubject)
        {
            throw new ForbiddenException($"Ви (Вчитель ID: {teacher.Id}) не викладаєте цей предмет (ID: {dto.SubjectId}).");
        }
        
        var letterValue = _gradeConverter.ConvertToLetter(dto.Value);
        
        var grade = Grade.CreateNew(dto.StudentId, dto.SubjectId, teacher.Id, dto.Value, letterValue);
        await _gradeRepository.AddAsync(grade);
    }

    public async Task UpdateGrade(UpdateGradeDto dto, int userId)
    {
        var validationResult = await _updateGradeValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }
        
        var grade = await _gradeRepository.GetById(dto.GradeId);
        
        if (grade == null) throw new GradeNotFoundException(dto.GradeId);

        await CheckGradeOwnership(grade.TeacherId, userId);

        var letterValue = _gradeConverter.ConvertToLetter(dto.Value);
        
        grade.UpdateNumericValue(dto.Value, letterValue);
        await _gradeRepository.SaveChangesAsync();
    }

    public async Task DeleteGrade(int gradeId, int userId)
    {
        var grade = await _gradeRepository.GetById(gradeId);
        
        if (grade == null) throw new GradeNotFoundException(gradeId);

        await CheckGradeOwnership(grade.TeacherId, userId);
        
        await _gradeRepository.DeleteAsync(grade);
    }

    public async Task<List<GradeResponseDto>> GetGradesForSubject(GetGradesQueryDto queryDto)
    {
        var validationResult = await _getGradeValidator.ValidateAsync(queryDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }
        
        var grades = await _gradeRepository.GetGradesByStudentAndSubject(queryDto.StudentId, queryDto.SubjectId);
        
        return grades.Select(g => new GradeResponseDto(g.NumericValue, g.LetterValue, g.CreatedAt)).ToList();
    }

    public async Task<StudentGpaResponseDto> CalculateStudentGpa(CalculateStudentGpaQueryDto dto)
    {
        var validationResult = await _calculateStudentGpaQueryValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }
        
        var student = await _studentRepository.GetById(dto.StudentId);
        if (student == null) throw new UserNotFoundException();

        var allGrades = await _gradeRepository.GetGradesByStudentAndSemester(dto.StudentId, dto.Semester);
    
        if (allGrades == null || !allGrades.Any())
        {
            return new StudentGpaResponseDto(dto.StudentId, 0.0m, new List<SubjectGpaDto>());
        }

        var subjectGpas = allGrades
            .GroupBy(g => g.SubjectId)
            .Select(group => new SubjectGpaDto(
                group.First().Subject.Name,
                Math.Round((decimal)group.Average(g => g.NumericValue), 2)
            ))
            .ToList();

        var overallGpa = Math.Round((decimal)allGrades.Average(g => g.NumericValue), 2);

        return new StudentGpaResponseDto(
            dto.StudentId,
            overallGpa,
            subjectGpas
        );
    }

    /// <summary>
    /// Перевірка прав доступу.
    /// Чи має право користувач виконати операцію з оцінкою.
    /// Чи є він вчителем, який виставив оцінку.
    /// </summary>
    /// <param name="gradeTeacherId"></param>
    /// <param name="currentUserId"></param>
    /// <exception cref="ForbiddenException"></exception>
    private async Task CheckGradeOwnership(int gradeTeacherId, int currentUserId)
    {
        var teacher = await _teacherRepository.GetByUserId(currentUserId);
    
        if (teacher == null)
        {
            throw new ForbiddenException();
        }
    
        if (gradeTeacherId != teacher.Id)
        {
            throw new ForbiddenException();
        }
    }
}