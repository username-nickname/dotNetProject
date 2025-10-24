namespace Application.DTO.Grade.Response;

public class SubjectGpaDto
{
    public string SubjectName { get; set; }
    public decimal AverageGrade { get; set; }

    public SubjectGpaDto(string subjectName, decimal averageGrade)
    {
        SubjectName = subjectName;
        AverageGrade = averageGrade;
    }
}