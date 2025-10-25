using Application.Interfaces.Services;
using Domain.Exceptions;
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

    [HttpGet("student/{studentId:int}/semester/{semester:int}"), AllowAnonymous]
    public async Task<IActionResult> GetStudentReport(int studentId,
        int semester)
    {
        var report =
            await _reportService.GetStudentReport(studentId, semester);
        
        if (report == null)
            throw new UserNotFoundException(
                $"Student with Id {studentId} not found");

        return OkDataResponse(report,
            "Student semester report");
    }

    [HttpGet("student/{studentId:int}/final")]
    public async Task<IActionResult> GetFinalStudentReport(int studentId)
    {
        var report = await _reportService.GetFinalReport(studentId);
        
        if (report == null)
            throw new UserNotFoundException(
                $"Student with Id {studentId} not found");

        return OkDataResponse(report,
            "Final student report.");
    }

    [HttpGet("group/")]
    public async Task<IActionResult> GetGroupReport(
        [FromQuery] string groupName,
        [FromQuery] int semester)
    {
        var report = await _reportService.GetGroupReport(groupName, semester);
        
        if (report == null)
            throw new UserNotFoundException(
                $"Group with name {groupName} not found");

        return OkDataResponse(report, "Group report.");
    }

    [HttpGet("group/statistics")]
    public async Task<IActionResult> GetGroupStatistics(
        [FromQuery] string groupName,
        [FromQuery] int semester
    )
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return BadRequest("Назва групи не може бути порожньою.");
        var stats =
            await _reportService.GetGroupStatistics(groupName, semester);

        if (stats == null)
            throw new UserNotFoundException(
                $"Group with name {groupName} not found");

        return OkDataResponse(stats, "Group statistics");
    }

    [HttpGet("teacher/{teacherId:int}/average")]
    public async Task<IActionResult> GetTeacherSubjectAverages(int teacherId)
    {
        var report = await _reportService.GetTeacherSubjectAverages(teacherId);

        if (report == null)
            throw new UserNotFoundException(
                $"Teacher with Id {teacherId} not found");

        return OkDataResponse(report, "Середній бал предметів вчителя");
    }

    [HttpGet("teacher/{teacherId:int}/semester-count")]
    public async Task<IActionResult> GetTeacherSemesterGradeCounts(
        int teacherId)
    {
        var report =
            await _reportService.GetTeacherSemesterGradeCounts(teacherId);

        if (report == null)
            throw new UserNotFoundException(
                $"Teacher with Id {teacherId} not found");
        
        return OkDataResponse(report, "Кількість оцінок вчителя за семестр");
    }

    [HttpGet("department/{departmentId:int}")]
    public async Task<IActionResult> GetDepartmentReport(int departmentId)
    {
        var report = await _reportService.GetDepartmentReport(departmentId);
        
        if (report == null)
            throw new UserNotFoundException(
                $"Department with Id {departmentId} not found");
        
        return OkDataResponse(report, "Загальна статистика кафедри");
    }
}