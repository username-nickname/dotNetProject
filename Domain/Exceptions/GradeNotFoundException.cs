namespace Domain.Exceptions;

public class GradeNotFoundException : Exception
{
    public GradeNotFoundException(int gradeId) : base($"Оцінку з ID {gradeId} не знайдено")
    {
        
    }
}