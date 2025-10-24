namespace Domain.Interfaces;

using Entities;

public interface ITeacherRepository
{
    Task Add(Teacher teacher);
    Task<bool> ExistsById(int id);
    Task<Teacher?> GetByUserId(int id);
}