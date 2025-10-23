namespace Domain.Interfaces;

public interface IPositionRepository
{
    Task<bool> ExistsById(int id);
}