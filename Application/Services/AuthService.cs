using Application.DTO.Auth.External;
using Application.Interfaces.External;
using Domain.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Exceptions;
using Application.DTO.Auth;
using Application.Interfaces;
using Application.Interfaces.Services;
using FluentValidation;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly IValidator<LoginUserDto> _loginValidator;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExternalAuthApi _authApi;

    public AuthService(
        IValidator<RegisterUserDto> registerValidator,
        IValidator<LoginUserDto> loginValidator,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IStudentRepository studentRepository,
        IDepartmentRepository departmentRepository,
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork,
        IExternalAuthApi authApi
    )
    {
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _teacherRepository = teacherRepository;
        _departmentRepository = departmentRepository;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _authApi = authApi;
    }

    public async Task<bool> Register(RegisterUserDto dto)
    {
        await _registerValidator.ValidateAndThrowAsync(dto);

        Enum.TryParse(dto.RoleName, ignoreCase: true, out RoleType role);

        var externalResponse = await _authApi.Register(new ExternalRegisterDto(dto.Email, dto.Password, dto.FullName, dto.RoleName));
        
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var user = User.CreateNew(externalResponse.Data.Email, externalResponse.Data.ExternalId, role);

            await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            switch (role)
            {
                case RoleType.Student:
                    await CreateStudentEntity(user.Id, dto.FullName, dto.Group, dto.YearOfEntry.Value);
                    break;

                case RoleType.Teacher:
                case RoleType.HeadOfDepartment:
                    var newTeacher = await CreateTeacherEntity(user.Id, dto.FullName, dto.DepartmentId.Value,
                        dto.PositionId.Value);

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
        _loginValidator.ValidateAndThrow(dto);

        var externalResponse = await _authApi.Login(new ExternalLoginDto(dto.Email, dto.Password));
        
        var user = await _userRepository.GetByExternalId(externalResponse.Data.ExternalId);
        if (user == null) throw new AuthenticationFailedException("Користувача не знайдено");

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