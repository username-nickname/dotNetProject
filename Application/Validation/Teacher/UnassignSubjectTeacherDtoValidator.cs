using Application.DTO.Teacher;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Teacher;

public class UnassignSubjectTeacherDtoValidator : AbstractValidator<UnassignSubjectTeacherDto>
{
    public UnassignSubjectTeacherDtoValidator(ITeacherRepository teacherRepository, ISubjectRepository subjectRepository)
    {
        RuleFor(x => x.TeacherId)
            .NotEmpty().WithMessage("Поле TeacherId обов'язкове")
            .MustAsync(async (teachId, token) =>
            {
                return await teacherRepository.ExistsById(teachId); 
            }).WithMessage("Вчитель не існує.");
        
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Предмет обв'язковий")
            .MustAsync(async (subjId, token) =>
            {
                return await subjectRepository.ExistsById(subjId); 
            }).WithMessage("Предмет не існує.");
        
        RuleFor(dto => dto)
            .MustAsync(async (dto, token) =>
            {
                return await teacherRepository.HasSubject(dto.TeacherId, dto.SubjectId);
            })
            .WithMessage("Цей предмет не призначено вчителю.");
    }
}