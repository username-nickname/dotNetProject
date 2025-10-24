using Domain.Interfaces;

namespace Domain.Entities;

public class Teacher : IAuditableEntity
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public int DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!; 
    
    public int PositionId { get; private set; }
    public Position Position { get; private set; } = null!;
    
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public ICollection<TeacherSubject> Subjects { get; private set; } = new List<TeacherSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Teacher() { }
    
    public static Teacher CreateNew(string fullName, int departmentId, int positionId, int userId)
    {
        ValidateRequiredString(fullName, nameof(fullName));
        return new Teacher
        {
            FullName = fullName, 
            DepartmentId = departmentId, 
            PositionId = positionId,
            UserId = userId
        };
    }
    
    private static void ValidateRequiredString(string value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле '{argumentName}' не може бути порожнім.", argumentName);
        }
    }
}