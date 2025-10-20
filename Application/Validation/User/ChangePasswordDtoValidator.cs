namespace Application.Validation.User;

using FluentValidation;
using DTO.User;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Password обов'язковий");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password обов'язковий")
            .MinimumLength(8).WithMessage("Password повинен містити від 8 до 20 символів")
            .MaximumLength(20).WithMessage("Password повинен містити від 8 до 20 символів");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Паролі не співпадають.");

    }
}