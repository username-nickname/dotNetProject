namespace Application.DTO.Reports;

public class DepartmentStatisticsDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int SubjectCount { get; set; }
    public double AverageGrade { get; set; }
    public IEnumerable<TopStudentDto> TopStudents { get; set; } = [];
}