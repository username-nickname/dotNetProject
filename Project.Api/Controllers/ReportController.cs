using Application.DTO.Reports.Query;
using Application.Interfaces.Services;
using Domain.Enums;
using Infrastructure.Security.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ApiControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("student/{studentId:int}/semester/{semester:int}")]
    // [Authorize]
    public async Task<IActionResult> GetStudentReport(int studentId,
        int semester)
    {
        var report =
            await _reportService.GetStudentReport(studentId, semester);

        return OkDataResponse(report,
            "Student semester report");
    }

    [HttpGet("student/{studentId:int}/final")]
    // [Authorize]
    public async Task<IActionResult> GetFinalStudentReport(int studentId)
    {
        var report = await _reportService.GetFinalReport(studentId);

        return OkDataResponse(report,
            "Final student report.");
    }

    [HttpGet("group/")]
    // [Authorize]
    public async Task<IActionResult> GetGroupReport(
        [FromQuery] GetGroupReportQueryDto dto)
    {
        var report = await _reportService.GetGroupReport(dto);

        return OkDataResponse(report, "Group report.");
    }

    [HttpGet("group/statistics")]
    // [Authorize]
    public async Task<IActionResult> GetGroupStatistics([FromQuery] GetGroupStatisticsQueryDto dto)
    {
        var stats =
            await _reportService.GetGroupStatistics(dto);

        return OkDataResponse(stats, "Group statistics");
    }

    [HttpGet("teacher/{teacherId:int}/average")]
    // [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> GetTeacherSubjectAverages(int teacherId)
    {
        var report = await _reportService.GetTeacherSubjectAverages(teacherId);

        return OkDataResponse(report, "Середній бал предметів вчителя");
    }

    [HttpGet("teacher/{teacherId:int}/semester-count")]
    // [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> GetTeacherSemesterGradeCounts(
        int teacherId)
    {
        var report =
            await _reportService.GetTeacherSemesterGradeCounts(teacherId);
        
        return OkDataResponse(report, "Кількість оцінок вчителя за семестр");
    }

    [HttpGet("department/{departmentId:int}")]
    // [RoleAuthorize(RoleType.HeadOfDepartment)]
    public async Task<IActionResult> GetDepartmentReport(int departmentId)
    {
        var report = await _reportService.GetDepartmentReport(departmentId);
        
        return OkDataResponse(report, "Загальна статистика кафедри");
    }
}