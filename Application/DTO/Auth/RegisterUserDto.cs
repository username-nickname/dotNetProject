namespace Application.DTO.Auth;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string RoleName { get; set; }
}