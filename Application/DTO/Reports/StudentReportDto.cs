namespace Application.DTO.Reports;

public class StudentReportDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Semester { get; set; }
    public double AverageGrade { get; set; }
    public string AverageGradeLetter { get; set; } = string.Empty;
    public bool IsPassed { get; set; }
    public List<StudentGradeDto> Grades { get; set; } = [];
}