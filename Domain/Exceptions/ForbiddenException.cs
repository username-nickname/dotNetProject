namespace Domain.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "У вас немає прав на виконання цієї операції") : base(message)
    {
    }
}