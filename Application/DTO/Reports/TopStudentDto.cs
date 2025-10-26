namespace Application.DTO.Reports;

public class TopStudentDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
}