namespace Application.DTO.Reports;

public class GroupStatisticsDto
{
    public string GroupName { get; set; } = null!;
    public int Semester { get; set; }
    public double AverageGroupGrade { get; set; }
    public int TotalStudents { get; set; }
    public int FailedCount { get; set; }
}