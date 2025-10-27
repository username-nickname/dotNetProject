namespace Application.DTO.Auth.External;

public class ExternalRegisterDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;

    public ExternalRegisterDto(string email, string password, string fullName, string role)
    {
        Email = email;
        Password = password;
        FullName = fullName;
        Role = role;
    }
}