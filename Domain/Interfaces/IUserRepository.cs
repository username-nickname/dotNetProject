namespace Domain.Interfaces;

using Entities;

public interface IUserRepository
{
    Task<bool> IsEmailTaken(string email);
    Task<User?> GetById(int id);
    Task<User?> GetByExternalId(string id);
    Task<User?> GetByEmail(string email);
    Task Add(User user);
    Task<int> GetTokenVersion(int userId);
    Task SaveChanges();
}