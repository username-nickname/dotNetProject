namespace Application.Services;

using Domain.Enums;
using Domain.Entities;
using Domain.Interfaces;
using DTO.Auth;
using Interfaces;
using Interfaces.Services;
using FluentValidation;

public class AuthService : IAuthService
{
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly IValidator<LoginUserDto> _loginValidator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    
    public AuthService(
        IValidator<RegisterUserDto> registerValidator,
        IValidator<LoginUserDto> loginValidator,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository
    )
    {
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }
    
    public async Task<bool> Register(RegisterUserDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }

        string passwordHash = _passwordHasher.HashPassword(dto.Password);
        Enum.TryParse(dto.RoleName, ignoreCase: true, out RoleType role);
        
        var user = User.CreateNew(dto.Email, dto.Name, dto.LastName, passwordHash, role);
        await _userRepository.Add(user);

        return true;
    }

    public async Task<string?> Login(LoginUserDto dto)
    {
        var validationResult = _loginValidator.Validate(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors); 
        }

        var user = await _userRepository.GetByEmail(dto.Email);

        if (user == null)
        {
            return null;
        }
        
        bool isPasswordValid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }
        
        return _tokenGenerator.GenerateToken(user);
    }
}