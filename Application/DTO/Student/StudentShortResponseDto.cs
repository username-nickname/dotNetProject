namespace Application.DTO.Student;

public class StudentShortResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Group { get; set; } = null!;
    public int YearOfEntry { get; set; }
}