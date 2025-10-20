namespace Application.Validation.User;

using FluentValidation;
using DTO.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEqual(string.Empty).WithMessage("Ім'я не може бути порожнім.")
            .MinimumLength(3).WithMessage("Ім'я повинно містити мінімум 3 символи.")
            .MaximumLength(50).WithMessage("Ім'я повинно містити максимум 50 символів.")
            .When(x => x.Name != null);
        
        RuleFor(x => x.LastName)
            .NotEqual(string.Empty).WithMessage("Прізвище не може бути порожнім.")
            .MinimumLength(3).WithMessage("Прізвище повинно містити мінімум 3 символи.")
            .MaximumLength(100).WithMessage("Прізвище повинно містити максимум 100 символів.")
            .When(x => x.LastName != null);
    }
}