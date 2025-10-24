using Application.DTO.Grade.Query;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Grade;

public class CalculateStudentGpaQueryDtoValidator: AbstractValidator<CalculateStudentGpaQueryDto>
{
    public CalculateStudentGpaQueryDtoValidator(
        IStudentRepository studentRepository
    )
    {
        RuleFor(x => x.StudentId)
            .MustAsync(async (studId, token) =>
            {
                return await studentRepository.ExistsById(studId);
            }).WithMessage("Студент не існує");

        RuleFor(x => x.Semester)
            .GreaterThan(0).WithMessage("Семестр має бути додатнім")
            .LessThan(3).WithMessage("Семестр має бути менше за 3");
    }
}