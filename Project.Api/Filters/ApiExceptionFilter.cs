namespace Project.Api.Filters;

using Microsoft.AspNetCore.Mvc;
using Application.DTO.Contracts;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// Глобальний обробник помилок
/// </summary>
public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        int statusCode;
        string message;
        List<ApiError> errors = new();

        switch (exception)
        {
            case ValidationException vex:
                statusCode = StatusCodes.Status400BadRequest;
                message = "Помилка валідації вхідних даних.";
                errors = vex.Errors.Select(e => new ApiError(e.PropertyName, e.ErrorMessage)).ToList();
                break;

            case UserNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
                break;
                
            case InvalidPasswordException or InvalidOperationException:
                statusCode = StatusCodes.Status400BadRequest;
                message = exception.Message;
                break;
            
            case ArgumentOutOfRangeException:
                statusCode = StatusCodes.Status400BadRequest;
                message = exception.Message;
                break;

            case DepartmentNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
                break;
            
            case GradeNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
                break;
            
            case ForbiddenException:
                statusCode = StatusCodes.Status403Forbidden;
                message = exception.Message;
                break;
            
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Непередбачена помилка сервера.";
                break;
        }

        var errorResponse = new ApiResponse<object>(
            Success: false,
            Message: message,
            Errors: errors
        );

        context.Result = new ObjectResult(errorResponse) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }
}