using Application.DTO.Reports;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Interfaces;

namespace Application.Services;

public class ReportService : IReportService
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IGradeConverter _gradeConverter;
    private readonly IDepartmentRepository _departmentRepository;

    public ReportService(IGradeRepository gradeRepository,
        IStudentRepository studentRepository,
        ITeacherRepository teacherRepository,
        ISubjectRepository subjectRepository,
        IGradeConverter gradeConverter,
        IDepartmentRepository departmentRepository
    )
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
        _subjectRepository = subjectRepository;
        _gradeConverter = gradeConverter;
        _departmentRepository = departmentRepository;
    }

    public async Task<StudentReportDto?> GetStudentReport(int studentId,
        int semester)
    {
        var student = await _studentRepository.GetByIdWithSubjects(studentId);
        if (student == null) return null;

        var grades =
            await _gradeRepository.GetGradesByStudentAndSemester(studentId,
                semester);
        if (grades.Count == 0)
            return new StudentReportDto
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Semester = semester,
                AverageGrade = 0,
                IsPassed = false,
                Grades = []
            };

        var subjects = student.Subjects.Select(s => s.Subject).ToList();

        var result = new List<StudentGradeDto>();
        foreach (var subject in subjects)
        {
            var subjectGrades =
                grades.Where(g => g.SubjectId == subject.Id).ToList();
            if (subjectGrades.Count == 0) continue;

            var avg = subjectGrades.Average(g => g.NumericValue);
            var letter =
                _gradeConverter.ConvertToLetter((int)Math.Round(avg));

            result.Add(new StudentGradeDto
            {
                SubjectName = subject.Name,
                NumericGrade = Math.Round(avg, 2),
                LetterGrade = letter
            });
        }

        var overall =
            result.Count > 0 ? result.Average(r => r.NumericGrade) : 0;
        var overallLetter =
            _gradeConverter.ConvertToLetter((int)Math.Round(overall));
        var isPassed = overall >= 35;

        return new StudentReportDto
        {
            StudentId = student.Id,
            FullName = student.FullName,
            Semester = semester,
            AverageGrade = Math.Round(overall, 2),
            IsPassed = isPassed,
            Grades = result
        };
    }

    public async Task<GroupReportDto?> GetGroupReport(string groupName,
        int semester)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return null;

        var group = await _studentRepository.GroupExistsByName(groupName);
        if (!group) return null;

        var students = await _studentRepository.GetByGroup(groupName);
        var result = new GroupReportDto
        {
            GroupName = groupName,
            Semester = semester,
            StudentReports = []
        };

        foreach (var student in students)
        {
            var report = await GetStudentReport(student.Id, semester);
            if (report != null)
                result.StudentReports.Add(report);
        }

        result.GroupAverage = result.StudentReports.Count > 0
            ? result.StudentReports.Average(r => r.AverageGrade)
            : 0;

        return result;
    }

    public async Task<FinalStudentReportDto?> GetFinalReport(int studentId)
    {
        var student = await _studentRepository.GetByIdWithSubjects(studentId);
        if (student == null) return null;

        var grades = await _gradeRepository.GetGradesByStudentId(studentId);
        if (grades.Count == 0)
            return new FinalStudentReportDto
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Semesters = []
            };

        var grouped = grades
            .GroupBy(g => g.Subject.Semester)
            .OrderBy(g => g.Key);

        var semesterSummaries = new List<SemesterSummaryDto>();
        foreach (var group in grouped)
        {
            var semesterGrades = group.ToList();
            var avg = semesterGrades.Average(g => g.NumericValue);

            semesterSummaries.Add(new SemesterSummaryDto
            {
                Semester = group.Key,
                AverageGrade = Math.Round(avg, 2),
                LetterGrade =
                    _gradeConverter.ConvertToLetter((int)Math.Round(avg)),
                Subjects = semesterGrades
                    .GroupBy(g => g.SubjectId)
                    .Select(sg =>
                    {
                        var subjectAvg = sg.Average(g => g.NumericValue);
                        return new StudentGradeDto
                        {
                            SubjectName = sg.First().Subject.Name,
                            NumericGrade = Math.Round(subjectAvg, 2),
                            LetterGrade =
                                _gradeConverter.ConvertToLetter(
                                    (int)Math.Round(subjectAvg))
                        };
                    })
                    .ToList()
            });
        }

        return new FinalStudentReportDto
        {
            StudentId = student.Id,
            FullName = student.FullName,
            Semesters = semesterSummaries
        };
    }

    public async Task<GroupStatisticsDto> GetGroupStatistics(string groupName,
        int semester)
    {
        var grades =
            await _gradeRepository.GetGradesByGroupAndSemester(groupName,
                semester);
        if (grades.Count == 0)
            return new GroupStatisticsDto
            {
                GroupName = groupName,
                Semester = semester,
                AverageGroupGrade = 0,
                FailedCount = 0,
                TotalStudents = 0
            };

        var studentIds = grades.Select(g => g.StudentId).Distinct().ToList();
        var totalStudents = studentIds.Count;

        var avgGroupGrade = grades.Average(g => g.NumericValue);

        var failedCount = studentIds.Count(studentId =>
        {
            var studentGrades = grades.Where(g => g.StudentId == studentId);
            return studentGrades.Any(g => g.NumericValue < 35);
        });

        return new GroupStatisticsDto
        {
            GroupName = groupName,
            Semester = semester,
            AverageGroupGrade = Math.Round(avgGroupGrade, 2),
            TotalStudents = totalStudents,
            FailedCount = failedCount
        };
    }

    public async Task<IEnumerable<TeacherSubjectAverageDto>?>
        GetTeacherSubjectAverages(int teacherId)
    {
        var teacher = await _teacherRepository.ExistsById(teacherId);
        if (!teacher)
            return null;

        var grades = (await _gradeRepository.GetGradesByTeacher(teacherId))
            .ToList();

        if (grades.Count == 0)
        {
            return null;
        }

        var report = grades
            .GroupBy(g => g.Subject)
            .Select(group => new TeacherSubjectAverageDto
            {
                SubjectName = group.Key.Name,
                AverageGrade = group.Average(g => g.NumericValue)
            })
            .OrderBy(dto => dto.SubjectName)
            .ToList();

        return report;
    }

    public async Task<IEnumerable<TeacherSemesterGradeCountDto>?>
        GetTeacherSemesterGradeCounts(int teacherId)
    {
        var teacher = await _teacherRepository.ExistsById(teacherId);
        if (!teacher)
            return null;

        var grades = (await _gradeRepository.GetGradesByTeacher(teacherId))
            .ToList();

        if (grades.Count == 0)
        {
            return null;
        }

        var report = grades
            .GroupBy(g => g.Subject.Semester)
            .Select(group => new TeacherSemesterGradeCountDto
            {
                Semester = group.Key,
                GradeCount = group.Count()
            })
            .OrderBy(dto => dto.Semester)
            .ToList();

        return report;
    }

    public async Task<DepartmentReportDto?> GetDepartmentReport(
        int departmentId)
    {
        var department = await _departmentRepository.ExistsById(departmentId);
        if (!department) return null;

        var grades =
            (await _gradeRepository.GetGradesByDepartment(departmentId))
            .ToList();
        var teacherCount =
            await _teacherRepository.CountByDepartment(departmentId);
        var subjectCount =
            await _subjectRepository.CountByDepartment(departmentId);
        var studentCount =
            await _studentRepository.CountByDepartment(departmentId);

        var averageGrade = grades.Count != 0
            ? grades.Average(g => g.NumericValue)
            : 0;

        return new DepartmentReportDto
        {
            DepartmentAverageGrade = averageGrade,
            TeacherCount = teacherCount,
            SubjectCount = subjectCount,
            StudentCount = studentCount
        };
    }
}