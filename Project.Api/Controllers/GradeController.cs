using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Grade;
using Application.DTO.Grade.Query;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Infrastructure.Security.Attributes;

namespace Project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ApiControllerBase
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }
    
    [HttpPost("add")]
    [RoleAuthorize(RoleType.Teacher)]
    public async Task<IActionResult> AddGrade([FromBody] AddGradeDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
        var userId = int.Parse(userIdString);
        
        await _gradeService.AddGrade(dto, userId);
        
        return OkResponse("Grade added");
    }
    
    [HttpPatch("update")]
    [RoleAuthorize(RoleType.Teacher)]
    public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
        var userId = int.Parse(userIdString);
        
        await _gradeService.UpdateGrade(dto, userId);
        
        return OkResponse("Grade updated");
    }
    
    [HttpDelete("{gradeId}/delete")]
    [RoleAuthorize(RoleType.Teacher)]
    public async Task<IActionResult> DeleteGrade(int gradeId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
        var userId = int.Parse(userIdString);
        
        await _gradeService.DeleteGrade(gradeId, userId);
        
        return OkResponse("Grade deleted");
    }
    
    [HttpGet("subject-grades")]
    [RoleAuthorize(RoleType.Teacher)]
    public async Task<IActionResult> GetGradesForSubject([FromQuery] GetGradesQueryDto queryDto)
    {
        var grades = await _gradeService.GetGradesForSubject(queryDto);
    
        return OkDataResponse(grades);
    }
    
    [HttpGet("gpa")]
    [Authorize(Roles = "Teacher")] 
    public async Task<IActionResult> CalculateGpa([FromQuery] CalculateStudentGpaQueryDto queryDto)
    {
        var gpaResult = await _gradeService.CalculateStudentGpa(queryDto);
        
        return OkDataResponse(gpaResult);
    }
}