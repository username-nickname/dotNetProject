using Domain.Enums;
using Infrastructure.Security.Attributes;

namespace Project.Api.Controllers;

using Application.DTO.Student;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ApiControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost("subjects/assign")]
    [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> AssignSubject([FromBody] AssignSubjectStudentDto dto)
    {
        await _studentService.AssignSubject(dto);
        
        return OkResponse("Subjects assigned successfully");
    }

    [HttpGet("{studentId}/subjects")]
    [Authorize]
    public async Task<IActionResult> GetStudentSubjects(int studentId)
    {
        var subjectDto = await _studentService.GetStudentSubjects(studentId);

        return OkDataResponse(subjectDto);
    }
}