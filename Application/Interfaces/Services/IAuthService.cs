namespace Application.Interfaces.Services;

using DTO.Auth;

public interface IAuthService
{
    Task<bool> Register(RegisterUserDto dto);
    Task<string?> Login(LoginUserDto dto);
}