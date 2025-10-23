namespace Application.DTO.Auth;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public string? Group { get; set; }
    public int? YearOfEntry { get; set; }
    public string RoleName { get; set; }
}