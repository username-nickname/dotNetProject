namespace Domain.Entities;

public class SubjectGrade
{
    public string SubjectName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int NumericValue { get; set; }
    public string LetterValue { get; set; } = string.Empty;
}