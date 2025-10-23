using Domain.Interfaces;

namespace Domain.Entities;

public class Teacher : IAuditableEntity
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public int DepartmentId { get; private set; } // Кафедра
    public Department Department { get; private set; } = null!; 
    
    public int PositionId { get; private set; } // посада
    public Position Position { get; private set; } = null!;
    
    public int UserId { get; private set; } // Связь с таблицей пользователей
    public User User { get; private set; } = null!;
    
    public ICollection<TeacherSubject> Subjects { get; private set; } = new List<TeacherSubject>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Teacher() { }
    
    public static Teacher CreateNew(string fullName, int departmentId, int positionId, int userId)
    {
        // TODO: (Валідація як в User.cs)
        return new Teacher
        {
            FullName = fullName, 
            DepartmentId = departmentId, 
            PositionId = positionId,
            UserId = userId
        };
    }
    
    // TODO: Реализовать методы для работы с свойствами (Напр. Добавить новый предмет, который ведет преподователь)
}