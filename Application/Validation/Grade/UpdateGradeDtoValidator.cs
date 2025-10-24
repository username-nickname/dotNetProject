using Application.DTO.Grade;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Grade;

public class UpdateGradeDtoValidator : AbstractValidator<UpdateGradeDto>
{
    public UpdateGradeDtoValidator(IGradeRepository gradeRepository)
    {
        RuleFor(x => x.GradeId)
            .NotEmpty().WithMessage("Поле GradeId обов'язкове")
            .MustAsync(async (id, token) =>
            {
                return await gradeRepository.ExistsById(id);
            }).WithMessage("Оцінки не існує");
        
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Оцінка обов'язкова.") 
            .GreaterThan(0).WithMessage("Оцінка має бути більше 0.")
            .LessThan(101).WithMessage("Оцінка має бути не більше 100.");
    }
}