using Application.DTO.Grade;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Grade;

public class AddGradeDtoValidator : AbstractValidator<AddGradeDto>
{
    public AddGradeDtoValidator(
        IStudentRepository studentRepository,
        ISubjectRepository subjectRepository
        )
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Поле StudentId обов'язкове")
            .MustAsync(async (studId, token) =>
            {
                return await studentRepository.ExistsById(studId);
            }).WithMessage("Студент не існує");
        
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Поле SubjectId обов'язкове")
            .MustAsync(async (subjId, token) =>
            {
                return await subjectRepository.ExistsById(subjId);
            }).WithMessage("Предмет не існує");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Оцінка обов'язкова.") 
            .GreaterThan(0).WithMessage("Оцінка має бути більше 0.")
            .LessThan(101).WithMessage("Оцінка має бути не більше 100.");
        
        RuleFor(dto => dto)
            .MustAsync(async (dto, token) =>
            {
                bool isAssigned = await studentRepository.HasSubject(dto.StudentId, dto.SubjectId);
                return isAssigned;
            })
            .WithMessage("Неможливо виставити оцінку. Предмет не призначено цьому студенту.");
    }
}