namespace Domain.Entities;

/// <summary>
/// Для таблиці Посад вчителів
/// </summary>
public class Position
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    private Position() { }
    
    public Position(int id, string name) 
    {
        Id = id;
        Name = name;
    }
}