namespace Application.Services;

using Domain.Interfaces;
using Domain.Exceptions;
using DTO.User;
using Interfaces.Services;
using FluentValidation;
using Interfaces;

public class UserService : IUserService
{
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public UserService(
        IValidator<ChangePasswordDto> changePasswordValidator,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher
        )
    {
        _changePasswordValidator = changePasswordValidator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<UserResponseDto> GetUserDetails(int userId)
    {
        var userEntity = await _userRepository.GetById(userId); 
    
        if (userEntity == null)
        {
            throw new UserNotFoundException();
        }
        
        var responseDto = new UserResponseDto
        {
            Id = userEntity.Id,
            Email = userEntity.Email,
        };
    
        return responseDto;
    }

    public async Task ChangePassword(ChangePasswordDto dto, int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null) throw new UserNotFoundException();

        _changePasswordValidator.ValidateAndThrow(dto);

        await PerformPasswordChecks(dto, user.PasswordHash); // VerifyPassword времязатратная функция, лучше сдать асинхронно
        
        user.ChangePassword(_passwordHasher.HashPassword(dto.NewPassword));
        user.IncrementTokenVersion(); // деактивация всех активных на данный момент токенов
        
        await _userRepository.SaveChanges();
    }
    
    private async Task PerformPasswordChecks(ChangePasswordDto dto, string originPasswordHash)
    {
        bool isOldPasswordCorrect = await Task.Run(() => 
            _passwordHasher.VerifyPassword(dto.OldPassword, originPasswordHash)
        ); 
    
        if (!isOldPasswordCorrect)
        {
            throw new InvalidPasswordException("The current password is incorrect."); 
        }
    
        bool isNewPasswordSameAsOld = await Task.Run(() => 
            _passwordHasher.VerifyPassword(dto.NewPassword, originPasswordHash)
        );
    
        if (isNewPasswordSameAsOld)
        {
            throw new InvalidPasswordException("The new password cannot match the current one..");
        }
    }
}