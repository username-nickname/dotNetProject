namespace Application.Interfaces;

using Domain.Entities;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}