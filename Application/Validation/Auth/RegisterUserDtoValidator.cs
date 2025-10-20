namespace Application.Validation.Auth;

using FluentValidation;
using Domain.Interfaces;
using DTO.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Поле Email повинно бути валідним email.")
            .MaximumLength(100).WithMessage("Email повинен бути до 100 символів");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name обов'язковий")
            .MinimumLength(3).WithMessage("Name повинен містити від 3 до 50 символів")
            .MaximumLength(50).WithMessage("Name повинен містити від 6 до 50 символів");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName обов'язковий")
            .MinimumLength(3).WithMessage("LastName повинен містити від 3 до 100 символів")
            .MaximumLength(100).WithMessage("LastName повинен містити від 6 до 100 символів");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password обов'язковий")
            .MinimumLength(8).WithMessage("Password повинен містити від 8 до 20 символів")
            .MaximumLength(20).WithMessage("Password повинен містити від 8 до 20 символів");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Паролі не співпадають.");

        // Перевірка, що юзера з такою поштою ще немає в БД
        RuleFor(x => x.Email)
            .MustAsync(async (email, token) =>
            {
                var isTaken = await userRepository.IsEmailTaken(email);
                return !isTaken;
            })
            .WithMessage("Користувач з таким Email вже зареєстрований.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Роль обов'язкова")
            .MustAsync(async (role, token) =>
            {
                var isExists = await roleRepository.ExistsByName(role);

                return isExists;
            })
            .WithMessage("Обраної ролі не існує");
    }
}