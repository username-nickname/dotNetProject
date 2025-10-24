using Application.DTO.Grade.Query;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validation.Grade;

public class GetGradesQueryDtoValidator : AbstractValidator<GetGradesQueryDto>
{
    public GetGradesQueryDtoValidator(
        IStudentRepository studentRepository,
        ISubjectRepository subjectRepository
    )
    {
        RuleFor(x => x.StudentId)
            .MustAsync(async (studId, token) =>
            {
                return await studentRepository.ExistsById(studId);
            }).WithMessage("Студент не існує");
        
        RuleFor(x => x.SubjectId)
            .MustAsync(async (subjId, token) =>
            {
                return await subjectRepository.ExistsById(subjId);
            }).WithMessage("Предмет не існує");
        
        RuleFor(dto => dto)
            .MustAsync(async (dto, token) =>
            {
                bool isAssigned = await studentRepository.HasSubject(dto.StudentId, dto.SubjectId);
                return isAssigned;
            })
            .WithMessage("Предмет не призначено цьому студенту.");
    }
}