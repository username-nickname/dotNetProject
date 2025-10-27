namespace Application.DTO.Auth.External;

public class ExternalRegisterResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public required ExternalUserDataDto Data { get; set; }
}