namespace Domain.Interfaces;

public interface IRoleRepository
{
    Task<bool> ExistsByName(string name);
}