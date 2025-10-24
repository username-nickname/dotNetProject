using Domain.Interfaces;

namespace Domain.Entities;

public class Student : IAuditableEntity
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Group { get; private set; } = null!;
    public int YearOfEntry { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public ICollection<StudentSubject> Subjects { get; private set; } = new List<StudentSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Student() { }

    public static Student CreateNew(string fullName, string group, int year, int userId) 
    {
        ValidateRequiredString(fullName, nameof(fullName));
        return new Student { FullName = fullName, Group = group, YearOfEntry = year, UserId = userId };
    }
    
    public void AssignSubject(int subjectId)
    {
        if (Subjects.Any(ss => ss.SubjectId == subjectId))
        {
            throw new InvalidOperationException("Предмет вже призначено цьому студенту.");
        }
    
        var studentSubject = new StudentSubject(Id, subjectId); 
        Subjects.Add(studentSubject);
    }
    
    private static void ValidateRequiredString(string value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле '{argumentName}' не може бути порожнім.", argumentName);
        }
    }
}