using Domain.Interfaces;

namespace Domain.Entities;

public class Subject : IAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int Semester { get; private set; } // Семестр викладання
    public int Credits { get; private set; } // Кількість кредитів
    
    // Список студентів, які вивчають предмет
    public ICollection<StudentSubject> Students { get; private set; } = new List<StudentSubject>();
    
    // Список викладачів, які викладають предмет
    public ICollection<TeacherSubject> Teachers { get; private set; } = new List<TeacherSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Subject() { }

    public static Subject CreateNew(string name, int semester, int credits)
    {
        // TODO: (Валідація як в User.cs)
        return new Subject { Name = name, Semester = semester, Credits = credits };
    }
}