using Application.DTO.Student;
using Application.DTO.Student.Query;
using Application.DTO.Subject;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;


namespace Application.Services;

public class StudentService : IStudentService
{
    private readonly IValidator<AssignSubjectStudentDto> _assignSubjectValidator;
    private readonly IStudentRepository _studentRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly ISubjectRepository _subjectRepository; // TODO: ?
    private readonly IValidator<StudentFilterDto> _validator;

    public StudentService(
        IValidator<AssignSubjectStudentDto> assignSubjectValidator,
        IStudentRepository studentRepository, 
        ISubjectRepository subjectRepository,
        IGradeRepository gradeRepository,
        IValidator<StudentFilterDto> validator
        )
    {
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
        _assignSubjectValidator = assignSubjectValidator;
        _gradeRepository = gradeRepository;
        _validator = validator;
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

    public async Task<IEnumerable<StudentDto>> GetFilteredStudents(StudentFilterDto filter)
    {
        await _validator.ValidateAndThrowAsync(filter);

        var studentQuery = _studentRepository.GetAsQueryable();
        var gradeQuery = _gradeRepository.GetAsQueryable();

        
        if (!string.IsNullOrWhiteSpace(filter.Group))
        {
            studentQuery = studentQuery.Where(s => s.Group == filter.Group);
        }

        if (filter.Semester.HasValue)
        {
            studentQuery = studentQuery.Where(s => 
                s.Subjects.Any(ss => ss.Subject.Semester == filter.Semester.Value));
        }

        var projectedQuery = studentQuery
            .Select(s => new StudentDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Group = s.Group,
                AverageGrade = gradeQuery
                    .Where(g => g.StudentId == s.Id)
                    .Average(g => (double?)g.NumericValue) ?? 0 
            });

        if (filter.MinAverageGrade.HasValue)
        {
            projectedQuery = projectedQuery.Where(dto => 
                dto.AverageGrade >= filter.MinAverageGrade.Value);
        }

        return projectedQuery.ToList();
    }
}