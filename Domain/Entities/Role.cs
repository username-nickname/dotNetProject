namespace Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    private Role() { }
    
    public Role(int id, string name) 
    {
        Id = id;
        Name = name;
    }
}