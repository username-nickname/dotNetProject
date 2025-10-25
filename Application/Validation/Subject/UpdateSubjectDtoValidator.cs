using Application.DTO.Subject;
using Domain.Interfaces;
using FluentValidation;


namespace Application.Validation.Subject;

public class UpdateSubjectDtoValidator : AbstractValidator<UpdateSubjectDto>
{
    public UpdateSubjectDtoValidator(ISubjectRepository subjectRepository)
    {
        RuleFor(x => x.Name)
            .NotEqual(string.Empty).WithMessage("Ім'я не може бути порожнім.")
            .MustAsync(async (name, token) =>
            {
                var isExists = await subjectRepository.ExistsByName(name);

                return !isExists;
            }).WithMessage("Предмет вже існує")
            .When(x => x.Name != null);
        
        RuleFor(x => x.Semester)
            .GreaterThan(0).WithMessage("Семестр має бути додатнім")
            .LessThan(9).WithMessage("Семестр має бути менше за 9")
            .When(x => x.Semester != null);
        
        RuleFor(x => x.Credits)
            .GreaterThan(0).WithMessage("Поле Credits має бути додатнім")
            .LessThan(101).WithMessage("Поле Credits має бути в межах від 1 до 100")
            .When(x => x.Semester != null);
    }
}