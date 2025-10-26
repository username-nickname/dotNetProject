using Application.DTO.Teacher;
using Application.Interfaces.Services;
using Domain.Enums;
using Infrastructure.Security.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ApiControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }
    
    [HttpPost("subjects/assign")]
    [RoleAuthorize(RoleType.HeadOfDepartment)]
    public async Task<IActionResult> AssignSubject([FromBody] AssignSubjectTeacherDto dto)
    {
        await _teacherService.AssignSubject(dto);
        
        return OkResponse("Subjects assigned successfully");
    }

    [HttpDelete("subjects/unassign")]
    [RoleAuthorize(RoleType.HeadOfDepartment)]
    public async Task<IActionResult> RemoveSubject([FromBody] UnassignSubjectTeacherDto dto)
    {
        await _teacherService.UnassignSubject(dto);
        
        return OkResponse("Subjects assigned successfully");
    }

    [HttpGet("{teacherId}/subjects/")]
    [RoleAuthorize(RoleType.Teacher,RoleType.HeadOfDepartment)]
    public async Task<IActionResult> GetAssignedSubjects(int teacherId)
    {
        var result = await _teacherService.GetAssignedSubjects(teacherId);
        
        return OkDataResponse(result);
    }

    [HttpGet("search")]
    [RoleAuthorize(RoleType.HeadOfDepartment)]
    public async Task<IActionResult> SearchTeachersByName([FromQuery] string name)
    {
        var teachers = await _teacherService.SearchTeachersByName(name);
        
        return OkDataResponse(teachers);
    }
}