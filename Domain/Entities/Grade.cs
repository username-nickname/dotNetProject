using Domain.Interfaces;

namespace Domain.Entities;

public class Grade : IAuditableEntity
{
    public int Id { get; private set; }
    public int StudentId { get; private set; }
    public Student Student { get; private set; } = null!;
    
    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;
    
    public int TeacherId { get; private set; }
    public Teacher Teacher { get; private set; } = null!;
    
    public int NumericValue { get; private set; } // Числова оцінка
    public string LetterValue { get; private set; } = null!; // Літерна оцінка (з'явиться пізніше)
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Grade() { }

    public static Grade CreateNew(int studentId, int subjectId, int teacherId, int value)
    {
        // TODO: (Валідація як в User.cs)
        
        return new Grade
        {
            StudentId = studentId, 
            SubjectId = subjectId, 
            TeacherId = teacherId, 
            NumericValue = value,
        };
    }
    
    // TODO:Реализовать методы для работы с свойствами (Напр. Добавить изменить оценку)
}