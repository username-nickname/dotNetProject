using Application.DTO.Student;
using Application.DTO.Subject;
using Application.Interfaces.Services;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class StudentService : IStudentService
{
    private readonly IValidator<AssignSubjectStudentDto> _assignSubjectValidator;
    private readonly IStudentRepository _studentRepository;
    private readonly ISubjectRepository _subjectRepository;

    public StudentService(
        IValidator<AssignSubjectStudentDto> assignSubjectValidator,
        IStudentRepository studentRepository, 
        ISubjectRepository subjectRepository
        )
    {
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
        _assignSubjectValidator = assignSubjectValidator;
    }
    
    public async Task AssignSubject(AssignSubjectStudentDto dto)
    {
        var validationResult = await _assignSubjectValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }
        
        var student = await _studentRepository.GetById(dto.StudentId);
        if (student == null) throw new UserNotFoundException();

        student.AssignSubject(dto.SubjectId); 
        
        await _studentRepository.SaveChangesAsync();
    }
    
    public async Task<List<SubjectResponseDto>> GetStudentSubjects(int studentId)
    {
        var student = await _studentRepository.GetByIdWithSubjects(studentId);
        if (student == null) throw new UserNotFoundException();
    
        var subjects = student.Subjects
            .Select(ss => new SubjectResponseDto(ss.Subject.Id, ss.Subject.Name, ss.Subject.Semester, ss.Subject.Credits))
            .ToList();
    
        return subjects; 
    }
    
    public async Task<IEnumerable<StudentShortResponseDto>> GetStudentsByGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            return [];
        }
        
        var students = await _studentRepository.GetByGroup(groupName);
        
        return students.Select(student => new StudentShortResponseDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Group = student.Group,
            YearOfEntry = student.YearOfEntry
        });
    }
}