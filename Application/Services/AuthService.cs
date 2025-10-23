namespace Application.Services;

using Domain.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Exceptions;
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
    private readonly IStudentRepository _studentRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthService(
        IValidator<RegisterUserDto> registerValidator,
        IValidator<LoginUserDto> loginValidator,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IStudentRepository studentRepository,
        IDepartmentRepository departmentRepository,
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork
    )
    {
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _teacherRepository = teacherRepository;
        _departmentRepository = departmentRepository;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
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
        
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var user = User.CreateNew(dto.Email, passwordHash, role);
            // TODO: Когда дадут АПИ ,удалить поле passwordHash из БД в тадлице юзеров (возможно еще email)
            await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            
            switch (role)
            {
                case RoleType.Student:
                    await CreateStudentEntity(user.Id, dto.FullName, dto.Group, dto.YearOfEntry.Value);
                    break;
                
                case RoleType.Teacher:
                case RoleType.HeadOfDepartment:
                    var newTeacher = await CreateTeacherEntity(user.Id, dto.FullName, dto.DepartmentId.Value, dto.PositionId.Value);

                    await _unitOfWork.SaveChangesAsync();
                    
                    if (role == RoleType.HeadOfDepartment)
                    {
                        await SetDepartmentHead(dto.DepartmentId.Value, newTeacher.Id);
                    }
                
                    break;
            }
        });

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
    
    private async Task CreateStudentEntity(int userId, string fullName, string group, int year)
    {
        var student = Student.CreateNew(fullName, group, year, userId);
        await _studentRepository.Add(student);
    }

    private async Task<Teacher> CreateTeacherEntity(int userId, string fullName, int departmentId, int positionId)
    {
        var teacher = Teacher.CreateNew(fullName, departmentId, positionId, userId);
        
        await _teacherRepository.Add(teacher);
        
        return teacher;
    }

    private async Task SetDepartmentHead(int departmentId, int teacherId)
    {
        var department = await _departmentRepository.GetById(departmentId);
        if (department == null) throw new DepartmentNotFoundException(departmentId);

        if (department.HeadTeacherId != null)
        {
            throw new InvalidOperationException($"Кафедра {department.Name} вже має завідувача.");
        }
        
        department.SetHead(teacherId);
    }
}