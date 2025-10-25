using Application.DTO.Subject;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Subject;

public class AddSubjectDtoValidator : AbstractValidator<AddSubjectDto>
{
    public AddSubjectDtoValidator(ISubjectRepository subjectRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва обов'язкова")
            .MustAsync(async (name, token) =>
            {
                var isExists = await subjectRepository.ExistsByName(name);

                return !isExists;
            }).WithMessage("Предмет вже існує");
        
        RuleFor(x => x.Semester)
            .GreaterThan(0).WithMessage("Семестр має бути додатнім")
            .LessThan(9).WithMessage("Семестр має бути менше за 9");
        
        RuleFor(x => x.Credits)
            .GreaterThan(0).WithMessage("Поле Credits має бути додатнім")
            .LessThan(101).WithMessage("Поле Credits має бути в межах від 1 до 100");
    }
}