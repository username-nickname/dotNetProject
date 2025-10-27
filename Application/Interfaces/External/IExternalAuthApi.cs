using Application.DTO.Auth.External;

namespace Application.Interfaces.External;

public interface IExternalAuthApi
{
    Task<ExternalRegisterResponseDto> Register(ExternalRegisterDto dto);
    Task<ExternalLoginResponseDto> Login(ExternalLoginDto dto);
}