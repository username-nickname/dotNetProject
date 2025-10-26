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
    
    public void AssignSubject(int subjectId)
    {
        if (Subjects.Any(ss => ss.SubjectId == subjectId))
        {
            throw new InvalidOperationException("Предмет вже призначено цьому вчителю.");
        }
    
        var teacherSubject = new TeacherSubject(Id, subjectId); 
        Subjects.Add(teacherSubject);
    }
    
    public void UnassignSubject(int subjectId)
    {
        var teacherSubject = Subjects
            .FirstOrDefault(ts => ts.SubjectId == subjectId);

        if (teacherSubject == null)
        {
            throw new InvalidOperationException("Цей предмет не призначено цьому вчителю.");
        }

        Subjects.Remove(teacherSubject);
    }
    
    private static void ValidateRequiredString(string value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле '{argumentName}' не може бути порожнім.", argumentName);
        }
    }
}