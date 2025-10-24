namespace Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message = "Користувача не знайдено")
        : base(message)
    {
    }
}