namespace Application.DTO.Subject;

public class AddSubjectDto
{
    public string Name { get; set; } = null!;
    public int Semester { get; set; }
    public int Credits { get; set; }
}