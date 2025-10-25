namespace Application.DTO.Reports;

public class SubjectGradeDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = null!;
    public int Semester { get; set; }
    public int NumericValue { get; set; }
    public string LetterValue { get; set; } = null!;
}