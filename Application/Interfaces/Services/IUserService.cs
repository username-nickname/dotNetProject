using Application.DTO.User;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseDto> GetUserDetails(int userId);
    Task ChangePassword(ChangePasswordDto dto, int userId);
}