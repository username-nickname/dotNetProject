namespace Application.DTO.Reports;

public class StudentSemesterReportDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public string Group { get; set; } = null!;
    public int Semester { get; set; }
    public int AcademicYear { get; set; }
    public List<SubjectGradeDto> Subjects { get; set; } = [];
    public double AverageGrade { get; set; }
}