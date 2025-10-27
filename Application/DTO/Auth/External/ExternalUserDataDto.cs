namespace Application.DTO.Auth.External;

public class ExternalUserDataDto
{
    public string ExternalId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
}