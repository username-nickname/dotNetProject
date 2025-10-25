namespace Domain.Entities;

public class StudentReport
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;

    public List<SubjectGrade> Subjects { get; set; } = [];
    public double AverageGrade { get; set; }

    private StudentReport()
    {
    }

    public static StudentReport CreateStudentReport(string studentName,
        string group, string department, List<SubjectGrade> subjects,
        double averageGrade)
    {
        return new StudentReport
        {
            StudentName = studentName,
            Group = group,
            Department = department,
            Subjects = subjects,
            AverageGrade = averageGrade
        };
    }
}