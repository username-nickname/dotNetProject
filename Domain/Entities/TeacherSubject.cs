namespace Domain.Entities;

/// <summary>
/// Таблица для связей Многие-Ко-Многим (Many to Many)
/// </summary>
public class TeacherSubject
{
    public int TeacherId { get; private set; }
    public Teacher Teacher { get; private set; } = null!;
    
    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;
    
    private TeacherSubject() {}
    public TeacherSubject(int teacherId, int subjectId)
    {
        TeacherId = teacherId;
        SubjectId = subjectId;
    }
}