namespace Domain.Interfaces;

public interface ISubjectRepository
{
    Task<bool> ExistsById(int id);
}