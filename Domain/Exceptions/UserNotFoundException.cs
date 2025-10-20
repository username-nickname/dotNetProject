namespace Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(int userId)
        : base($"Користувача з ID {userId} не знайдено.")
    {
    }
}