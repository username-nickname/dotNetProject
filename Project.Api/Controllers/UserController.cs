namespace Project.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTO.User;
using Application.Interfaces.Services;
using FluentValidation;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var userDto = await _userService.GetUserDetails(userId);
        
            return OkDataResponse(userDto);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
            var userId = int.Parse(userIdString);
            
            await _userService.UpdateUser(dto, userId);

            return OkResponse("User updated successfully");
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }));
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
    
            var userId = int.Parse(userIdString);
            
            await _userService.ChangePassword(dto, userId);

            return OkResponse("Password changed successfully");
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }));
        }
        catch (InvalidPasswordException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}