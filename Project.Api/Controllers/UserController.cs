namespace Project.Api.Controllers;

using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Infrastructure.Security.Attributes;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{userId}")] // api/user/1
    [RoleAuthorize(RoleType.Student)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var userDto = await _userService.GetUserDetails(userId);
        
        return OkDataResponse(userDto);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
        var userId = int.Parse(userIdString);
            
        await _userService.ChangePassword(dto, userId);

        return OkResponse("Password changed successfully");
    }
}