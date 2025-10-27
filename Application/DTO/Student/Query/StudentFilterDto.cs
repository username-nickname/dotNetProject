namespace Application.DTO.Student.Query;

public class StudentFilterDto
{
    public string? Group { get; set; }
    public int? Semester { get; set; }
    public double? MinAverageGrade { get; set; }
}