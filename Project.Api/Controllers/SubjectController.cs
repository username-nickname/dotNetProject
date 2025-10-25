using Application.DTO.Subject;
using Domain.Enums;
using Infrastructure.Security.Attributes;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace Project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ApiControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }
    
    [HttpPost("add/")]
    [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> AddSubject([FromBody] AddSubjectDto dto)
    {
        await _subjectService.AddSubject(dto);
        
        return OkResponse("Subjects added successfully");
    }
    
    [HttpPatch("{subjectId}/update")]
    [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectDto dto, int subjectId)
    {
        await _subjectService.UpdateSubject(dto, subjectId);
        
        return OkResponse("Subjects updated successfully");
    }
    
    [HttpGet]
    [RoleAuthorize(RoleType.Teacher, RoleType.HeadOfDepartment)]
    public async Task<IActionResult> GetSubjects()
    {
        var result = await _subjectService.GetAllSubjects();

        return OkDataResponse(result);
    }
}