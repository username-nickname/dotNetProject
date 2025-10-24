namespace Application.DTO.Grade.Response;

public class StudentGpaResponseDto
{
    public int StudentId { get; init; }
    public decimal OverallGpa { get; init; }
    public List<SubjectGpaDto> SubjectGpas { get; init; } = new List<SubjectGpaDto>();

    public StudentGpaResponseDto(int studentId, decimal overallGpa, List<SubjectGpaDto> subjectGpas)
    {
        StudentId = studentId;
        OverallGpa = overallGpa;
        SubjectGpas = subjectGpas;
    }
}