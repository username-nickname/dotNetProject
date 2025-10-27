namespace Domain.Entities;

using Enums;
using Interfaces;

public class User : IAuditableEntity
{
    public int Id { get; private set; }
    public string ExternalId { get; private set; }
    public string Email { get; private set; }
    public int TokenVersion { get; private set; } = 1;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public int RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    
    private User() { } 
    
    public static User CreateNew(string email, string externalId, RoleType role)
    {
        ValidateRequiredString(email, nameof(email));
        ValidateRequiredString(externalId, nameof(externalId));
        
        return new User
        {
            Email = email,
            ExternalId = externalId,
            RoleId = (int)role
        };
    }
    
    public bool HasRole(RoleType requiredRole)
    {
        return RoleId == (int)requiredRole;
    }
    
    public void IncrementTokenVersion()
    {
        TokenVersion++;
    }
    
    private static void ValidateRequiredString(string value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле '{argumentName}' не може бути порожнім.", argumentName);
        }
    }
}