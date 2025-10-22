namespace Infrastructure.Security.Attributes;

using Microsoft.AspNetCore.Authorization;
using Domain.Enums;

/// <summary>
/// Атрибут для передачи в него масива ролей юзеров в виде энама, которые должны иметь доступ к маршруту
/// </summary>
public class RoleAuthorize : AuthorizeAttribute
{
    public RoleAuthorize(params RoleType[] roles)
    {
        var roleNames = roles.Select(r => r.ToString());
        
        Roles = string.Join(",", roleNames);
    }
}