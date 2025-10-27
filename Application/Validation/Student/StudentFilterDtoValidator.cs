using Application.DTO.Student.Query;
using FluentValidation;

namespace Application.Validation.Student;

public class StudentFilterDtoValidator : AbstractValidator<StudentFilterDto>
{
    public StudentFilterDtoValidator()
    {
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