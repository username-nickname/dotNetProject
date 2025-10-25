namespace Domain.Exceptions;

public class SubjectNotFoundException : Exception
{
    public SubjectNotFoundException(int subjectId) : base($"Предмет з ID {subjectId} не знайдено")
    {
    }
}