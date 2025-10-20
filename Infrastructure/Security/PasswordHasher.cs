namespace Infrastructure.Security;

using Application.Interfaces;
using BCrypt.Net;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password, workFactor: 12);
        
    }

    public bool VerifyPassword(string providedPassword, string hashedPassword)
    {
        return BCrypt.Verify(providedPassword, hashedPassword);
    }
}