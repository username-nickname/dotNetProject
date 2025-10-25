namespace Application.DTO.Reports;

public class SemesterSummaryDto
{
    public int Semester { get; set; }
    public double AverageGrade { get; set; }
    public string LetterGrade { get; set; } = null!;
    public List<StudentGradeDto> Subjects { get; set; } = [];
}