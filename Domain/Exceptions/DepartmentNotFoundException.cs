namespace Domain.Exceptions;

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException(int departmentId) : base($"Кафедру з ID {departmentId} не знайдено.")
    { }
}