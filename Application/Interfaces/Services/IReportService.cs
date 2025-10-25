using Application.DTO.Reports;
using Application.DTO.Reports.Query;

namespace Application.Interfaces.Services;

public interface IReportService
{
    Task<StudentReportDto> GetStudentReport(int studentId, int semester);
    Task<GroupReportDto> GetGroupReport(GetGroupReportQueryDto dto);
    Task<FinalStudentReportDto> GetFinalReport(int studentId);
    Task<GroupStatisticsDto> GetGroupStatistics(GetGroupStatisticsQueryDto dto);
    Task<IEnumerable<TeacherSubjectAverageDto>> GetTeacherSubjectAverages(int teacherId);
    Task<IEnumerable<TeacherSemesterGradeCountDto>> GetTeacherSemesterGradeCounts(int teacherId);
    Task<DepartmentReportDto> GetDepartmentReport(int departmentId);
}