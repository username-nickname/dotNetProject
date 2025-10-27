namespace Infrastructure.Seeding;

public class UserSeedDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RoleSeedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class PositionSeedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class SubjectSeedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Semester { get; set; }
    public int Credits { get; set; }
}

public class DepartmentSeedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? HeadTeacherId { get; set; }
}

public class TeacherSeedDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int UserId { get; set; }
}

public class StudentSeedDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Group { get; set; } = null!;
    public int YearOfEntry { get; set; }
    public int UserId { get; set; }
}

public class GradeSeedDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int NumericValue { get; set; }
    public string LetterValue { get; set; } = null!;
}

public class StudentSubjectSeedDto
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
}

public class TeacherSubjectSeedDto
{
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
}