using Application.DTO.Reports.Query;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Report;

public class GetGroupReportQueryDtoValidator: AbstractValidator<GetGroupReportQueryDto>
{
    public GetGroupReportQueryDtoValidator(
        IStudentRepository studentRepository
        )
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("Група обов'язкова.")
            .MaximumLength(100).WithMessage("Група не може бути більше 100 символів")
            .MustAsync(async (group, token) =>
            {
                return await studentRepository.GroupExistsByName(group);
            }).WithMessage("Група не існує");
        
        RuleFor(x => x.Semester)
            .GreaterThan(0).WithMessage("Семестр має бути додатнім")
            .LessThan(9).WithMessage("Семестр має бути менше за 9");
    }
}