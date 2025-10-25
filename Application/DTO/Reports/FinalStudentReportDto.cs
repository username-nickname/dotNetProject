namespace Application.DTO.Reports;

public class FinalStudentReportDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = null!;
    public List<SemesterSummaryDto> Semesters { get; set; } = [];
}