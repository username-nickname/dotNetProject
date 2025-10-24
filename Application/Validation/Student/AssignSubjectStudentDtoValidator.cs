using Application.DTO.Student;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Student;

public class AssignSubjectStudentDtoValidator : AbstractValidator<AssignSubjectStudentDto>
{
    public AssignSubjectStudentDtoValidator(IStudentRepository studentRepository, ISubjectRepository subjectRepository)
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Поле StudentId обов'язкове")
        .MustAsync(async (studId, token) =>
        {
            return await studentRepository.ExistsById(studId); 
        }).WithMessage("Студент не існує.");
        
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Предмет обв'язковий")
            .MustAsync(async (subjId, token) =>
            {
                return await subjectRepository.ExistsById(subjId); 
            }).WithMessage("Предмет не існує.");
        
        RuleFor(dto => dto)
            .MustAsync(async (dto, token) =>
            {
                bool alreadyAssigned = await studentRepository.HasSubject(dto.StudentId, dto.SubjectId);
                return !alreadyAssigned;
            })
            .WithMessage("Цей предмет уже призначено студенту.");
    }
}