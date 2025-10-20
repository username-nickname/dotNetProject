namespace Application.Validation.Auth;

using FluentValidation;
using DTO.Auth;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Поле Email повинно бути валідним email.")
            .MaximumLength(100).WithMessage("Email повинен бути до 100 символів");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password обов'язковий");
    }
}