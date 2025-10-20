namespace Project.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTO.Auth;
using Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/")]
public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        await _authService.Register(dto);
        return OkResponse("Register successfully");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
    {
        try
        {
            var token= await _authService.Login(dto);
            
            if (token == null)
            {
                return Unauthorized(new { message = "Incorrect Email or Password." });
            }

            return OkDataResponse(new { token = token });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", data = ex.Message });
        }
    }
}