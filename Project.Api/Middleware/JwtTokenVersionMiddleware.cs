namespace Project.Api.Middleware;

using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class JwtTokenVersionMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenVersionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Проверка ,активен ли токен.
    /// Если версия токена ниже tokenVersion пользователя в таблице users - значит токен был декатирован (смена пароля/завершение всех сессий)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userRepository"></param>
    public async Task InvokeAsync(HttpContext context, 
        IUserRepository userRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tokenVersionClaim = context.User.FindFirst("tver");
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        
            if (tokenVersionClaim != null && userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out int userId) &&
                    int.TryParse(tokenVersionClaim.Value, out int tokenVersion))
                {
                    var actualVersion = await userRepository.GetTokenVersion(userId); 
                    
                    if (actualVersion > tokenVersion)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        
                        return; 
                    }
                }
            }
        }

        await _next(context);
    }
}