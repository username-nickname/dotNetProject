namespace Project.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTO.Contracts;

/// <summary>
/// Базовий API контроллер.
/// Надає доступ до методів формування відповіді від роуту у фіксованому форматі
/// </summary>
public class ApiControllerBase : ControllerBase
{
    protected IActionResult OkDataResponse<T>(T data, string message = "Success.")
    {
        return Ok(new ApiResponse<T>(
            Success: true,
            Message: message,
            Data: data
        ));
    }
    
    protected IActionResult OkResponse(string message = "Success.")
    {
        return OkDataResponse<object>(data: null, message);
    }
}