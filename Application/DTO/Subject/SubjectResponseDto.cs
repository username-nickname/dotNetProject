namespace Application.DTO.Subject;

public class SubjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public SubjectResponseDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}