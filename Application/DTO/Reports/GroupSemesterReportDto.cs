namespace Application.DTO.Reports;

public class GroupSemesterReportDto
{
    public string Group { get; set; } = null!;
    public int Semester { get; set; }
    public int AcademicYear { get; set; }
    public double AverageGrade { get; set; }
    public int StudentsCount { get; set; }
    public int StudentsFailedCount { get; set; }
}