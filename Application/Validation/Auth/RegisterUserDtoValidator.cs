using Domain.Enums;
using FluentValidation;
using Domain.Interfaces;
using Application.DTO.Auth;

namespace Application.Validation.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IDepartmentRepository departmentRepository,
        IPositionRepository positionRepository
        )
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Поле Email повинно бути валідним email.")
            .MaximumLength(100).WithMessage("Email повинен бути до 100 символів")
            .MustAsync(async (email, token) =>
            {
                var isTaken = await userRepository.IsEmailTaken(email);
                return !isTaken;
            }).WithMessage("Користувач з таким Email вже зареєстрований.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName обов'язковий")
            .MinimumLength(10).WithMessage("FullName повинен містити від 10 до 100 символів")
            .MaximumLength(100).WithMessage("FullName повинен містити не більше 100 символів");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password обов'язковий")
            .MinimumLength(8).WithMessage("Password повинен містити від 8 до 20 символів")
            .MaximumLength(20).WithMessage("Password повинен містити від 8 до 20 символів");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Паролі не співпадають.");
        
        When(x => x.RoleName.Equals(RoleType.Student.ToString(), StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.Group)
                .NotEmpty().WithMessage("Для студента група обов'язкова.")
                .MaximumLength(100);
            RuleFor(x => x.YearOfEntry)
                .NotEmpty().WithMessage("Для студента рік вступу обов'язковий.")
                .GreaterThan(2000).WithMessage("Рік вступу має бути реалістичним.");
        });
        
        When(x => x.RoleName.Equals(RoleType.Teacher.ToString(), StringComparison.OrdinalIgnoreCase) ||
                  x.RoleName.Equals(RoleType.HeadOfDepartment.ToString(), StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Для викладача ID кафедри обов'язковий.")
                .MustAsync(async (deptId, token) => {
                    if (!deptId.HasValue) return false;
                    return await departmentRepository.ExistsById(deptId.Value); 
                }).WithMessage("Вказана кафедра не існує.");

                RuleFor(x => x.PositionId)
                    .NotEmpty().WithMessage("Посада обов'язкова.")
                .MustAsync(async (posId, token) => {
                        if (!posId.HasValue) return false;
                    return await positionRepository.ExistsById(posId.Value); 
                }).WithMessage("Посада не існує.");
        });
        
        When(x => x.RoleName.Equals(RoleType.HeadOfDepartment.ToString(), StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.PositionId)
                .Must(positionId => 
                {
                    // Перевіряємо, що ID посади не є Асистентом
                    if (positionId == (int)PositionType.Assistant)
                    {
                        return false; 
                    }
                    return true;
                })
                .WithMessage("Завідувачем кафедри може бути лише Доцент або Професор.");
        });

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