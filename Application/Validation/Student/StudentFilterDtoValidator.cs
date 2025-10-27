using Application.DTO.Student.Query;
using FluentValidation;

namespace Application.Validation.Student;

public class StudentFilterDtoValidator : AbstractValidator<StudentFilterDto>
{
    public StudentFilterDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Name повинен містити від 3 до 50 символів")
            .MaximumLength(50).WithMessage("Name повинен містити не більше 50 символів")
            .When(x => x.Name != null);
        
        RuleFor(x => x.Group)
            .MaximumLength(100).WithMessage("Група має містити не більше 100 символів")
            .MaximumLength(50).WithMessage("Name повинен містити не більше 50 символів")
            .When(x => x.Group != null);
        
        RuleFor(x => x.Semester)
            .InclusiveBetween(1, 8)
            .When(x => x.Semester.HasValue)
            .WithMessage("Семестр має бути в діапазоні 1-8.");

        RuleFor(x => x.MinAverageGrade)
            .InclusiveBetween(0, 100)
            .When(x => x.MinAverageGrade.HasValue)
            .WithMessage("Середній бал має бути 0-100");
    }
}