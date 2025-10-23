using Domain.Interfaces;

namespace Domain.Entities;

public class Student : IAuditableEntity
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Group { get; private set; } = null!;
    public int YearOfEntry { get; private set; }
    
    public int UserId { get; private set; } // Связь с таблицей пользователей
    public User User { get; private set; } = null!;
    
    public ICollection<StudentSubject> Subjects { get; private set; } = new List<StudentSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Student() { }

    public static Student CreateNew(string fullName, string group, int year, int userId) 
    {
        // TODO: (Валідація як в User.cs)
        return new Student { FullName = fullName, Group = group, YearOfEntry = year, UserId = userId };
    }
    
    // TODO: Реализовать методы для работы с свойствами (Напр. Добавить новый предмет, который изучает студент)
}