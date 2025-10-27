namespace Application.DTO.Auth.External;

public class ExternalLoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public required ExternalUserDataDto Data { get; set; }
}