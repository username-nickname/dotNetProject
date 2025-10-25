namespace Domain.Interfaces;

using Entities;

public interface IDepartmentRepository
{
    Task<Department?> GetById(int id);
    Task<bool> ExistsById(int id);
    Task SaveChanges();
}