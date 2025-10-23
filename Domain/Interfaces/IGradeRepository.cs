using Domain.Entities;

namespace Domain.Interfaces;

public interface IGradeRepository
{
    Task<Grade?> GetById(int id);
    Task AddAsync(Grade grade);
    Task SaveChangesAsync();
}