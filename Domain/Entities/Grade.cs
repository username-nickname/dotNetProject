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
    
    public int NumericValue { get; private set; }
    public string LetterValue { get; private set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Grade() { }

    public static Grade CreateNew(int studentId, int subjectId, int teacherId, int numericValue, string letterValue)
    {
        return new Grade
        {
            StudentId = studentId, 
            SubjectId = subjectId, 
            TeacherId = teacherId, 
            NumericValue = numericValue,
            LetterValue = letterValue,
        };
    }
    
    public void UpdateNumericValue(int newValue, string newLetterValue)
    {
        if (newValue < 0 || newValue > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(newValue), "Оцінка має бути в діапазоні 0-100.");
        }
    
        NumericValue = newValue;
        LetterValue = newLetterValue;
    }
}