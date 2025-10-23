namespace Domain.Entities;

public class StudentSubject
{
    public int StudentId { get; private set; }
    public Student Student { get; private set; } = null!;
    
    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;
    
    private StudentSubject() {}
    public StudentSubject(int studentId, int subjectId) 
    {
        StudentId = studentId;
        SubjectId = subjectId;
    }
}