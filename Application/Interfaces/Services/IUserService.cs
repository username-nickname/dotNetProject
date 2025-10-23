namespace Application.Interfaces.Services;

using DTO.User;

public interface IUserService
{
    Task<UserResponseDto> GetUserDetails(int userId);
    Task ChangePassword(ChangePasswordDto dto, int userId);
}