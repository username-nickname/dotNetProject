namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task ExecuteInTransactionAsync(Func<Task> action);
    Task SaveChangesAsync();
}