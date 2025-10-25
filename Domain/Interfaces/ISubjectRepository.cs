using Domain.Entities;

namespace Domain.Interfaces;

public interface ISubjectRepository
{
    Task<Subject?> GetById(int id);
    Task Add(Subject subject);
    Task<bool> ExistsById(int id);
    Task<bool> ExistsByName(string name);
    Task<IEnumerable<Subject>> GetAll();
    Task<int> CountByDepartment(int departmentId);
    Task SaveChanges();
}