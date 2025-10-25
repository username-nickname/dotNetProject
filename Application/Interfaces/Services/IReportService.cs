using Application.DTO.Reports;

namespace Application.Interfaces.Services;

public interface IReportService
{
    Task<StudentReportDto?> GetStudentReport(int studentId, int semester);
    Task<GroupReportDto?> GetGroupReport(string groupName, int semester);
    Task<FinalStudentReportDto?> GetFinalReport(int studentId);
    Task<GroupStatisticsDto> GetGroupStatistics(string groupName, int semester);
    Task<IEnumerable<TeacherSubjectAverageDto>?> GetTeacherSubjectAverages(int teacherId);
    Task<IEnumerable<TeacherSemesterGradeCountDto>?> GetTeacherSemesterGradeCounts(int teacherId);
    Task<DepartmentReportDto?> GetDepartmentReport(int departmentId);
}