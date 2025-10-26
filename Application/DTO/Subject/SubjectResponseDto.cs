namespace Application.DTO.Subject;

public class SubjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Semester { get; set; }
    public int Credits { get; set; }

    public SubjectResponseDto(int id, string name, int semester, int credits)
    {
        Id = id;
        Name = name;
        Semester = semester;
        Credits = credits;
    }
}