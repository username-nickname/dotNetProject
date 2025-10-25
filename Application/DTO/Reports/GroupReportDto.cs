namespace Application.DTO.Reports;

public class GroupReportDto
{
    public string GroupName { get; set; } = null!;
    public int Semester { get; set; }
    public double GroupAverage { get; set; }
    public List<StudentReportDto> StudentReports { get; set; } = new();
}