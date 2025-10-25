namespace Application.DTO.Reports;

public class StudentGradeDto
{
    public string SubjectName { get; set; } = null!;
    public double NumericGrade  { get; set; }
    public string LetterGrade { get; set; } = null!;
}