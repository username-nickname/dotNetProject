using Domain.Interfaces;

namespace Domain.Entities;

public class Subject : IAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int Semester { get; private set; }
    public int Credits { get; private set; }
    
    // Список студентів, які вивчають предмет
    public ICollection<StudentSubject> Students { get; private set; } = new List<StudentSubject>();
    
    // Список викладачів, які викладають предмет
    public ICollection<TeacherSubject> Teachers { get; private set; } = new List<TeacherSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Subject() { }

    public Subject(int id, string name, int semester, int credits)
    {
        // TODO: Возможно позже переделать создание через маршруты, а не дефолт. знач. в БД
        Id = id;
        Name = name;
        Semester = semester;
        Credits = credits;
    }
    
    public static Subject CreateNew(string name, int semester, int credits)
    {
        ValidateRequiredString(name,  nameof(name));
        return new Subject { Name = name, Semester = semester, Credits = credits };
    }
    
    private static void ValidateRequiredString(string value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле '{argumentName}' не може бути порожнім.", argumentName);
        }
    }
}