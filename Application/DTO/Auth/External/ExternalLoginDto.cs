namespace Application.DTO.Auth.External;

public class ExternalLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public ExternalLoginDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}