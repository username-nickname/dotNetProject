using Domain.Interfaces;

namespace Domain.Entities;

public class Department : IAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int? HeadTeacherId { get; private set; } // Завідувач (може бути нулл, потрібно для розірвання циклічної залежності при реєстрації)
    public Teacher? HeadTeacher { get; private set; }
    
    public ICollection<Teacher> Teachers { get; private set; } = new List<Teacher>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private Department() { }
    
    public Department(int id, string name, int? headTeacherId) 
    {
        Id = id;
        Name = name;
        HeadTeacherId = headTeacherId;
    }
    
    public static Department CreateNew(string name, int headTeacherId)
    {
        return new Department
        {
            Name = name, 
            HeadTeacherId = headTeacherId
        };
    }
    
    public void SetHead(int teacherId)
    {
        HeadTeacherId = teacherId;
    }
}