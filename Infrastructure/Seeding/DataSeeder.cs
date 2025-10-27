using System.Reflection;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeding;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<DataSeeder> _logger;
    private readonly string _seedDataPath;

    public DataSeeder(AppDbContext context, IPasswordHasher passwordHasher,
        ILogger<DataSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;

        var baseDir = AppContext.BaseDirectory;
        _seedDataPath = Path.Combine(baseDir, "SeedData");

        _logger.LogInformation("SeedData Path: {Path}", _seedDataPath);
    }

    public async Task Seed()
    {
        try
        {
            await SeedRolesAsync();
            await SeedPositionsAsync();
            await SeedSubjectsAsync();

            await SeedUsersAsync();
            await SeedDepartmentsAsync();

            await SeedTeachersAsync();
            await SeedStudentsAsync();

            await UpdateDepartmentHeadsAsync();

            await SeedTeacherSubjectsAsync();
            await SeedStudentSubjectsAsync();

            await SeedGradesAsync();

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding db");
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<RoleSeedDto>>("roles.json");
        if (dtos == null) return;

        var entities = dtos.Select(dto => new Role(dto.Id, dto.Name));
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Roles] ON;");

            await _context.Roles.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Roles] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Positions");
            throw;
        }
    }

    private async Task SeedPositionsAsync()
    {
        if (await _context.Positions.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<PositionSeedDto>>("positions.json");
        if (dtos == null) return;

        var entities = dtos.Select(dto => new Position(dto.Id, dto.Name));
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Positions] ON;");

            await _context.Positions.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Positions] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Positions");
            throw;
        }
    }

    private async Task SeedSubjectsAsync()
    {
        if (await _context.Subjects.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<SubjectSeedDto>>("subjects.json");
        if (dtos == null) return;

        var entities = dtos.Select(dto =>
            new Subject(dto.Id, dto.Name, dto.Semester, dto.Credits));
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Subjects] ON;");

            await _context.Subjects.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Subjects] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Subjects");
            throw;
        }
    }

    private async Task SeedUsersAsync()
    {
        if (await _context.Users.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<UserSeedDto>>("users.json");
        if (dtos == null) return;

        var entities = new List<User>();
        foreach (var dto in dtos)
        {
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            var entity = User.CreateNew(dto.Email, passwordHash,
                (RoleType)dto.RoleId);

            SetPrivateProperty(entity, "Id", dto.Id); 

            entities.Add(entity);
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Users] ON;");

            await _context.Users.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Users] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Users");
            throw;
        }
    }

    private async Task SeedDepartmentsAsync()
    {
        if (await _context.Departments.AnyAsync()) return;
        var dtos =
            await LoadJsonAsync<List<DepartmentSeedDto>>("departments.json");
        if (dtos == null) return;

        var entities =
            dtos.Select(dto => new Department(dto.Id, dto.Name, null));
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Departments] ON;");

            await _context.Departments.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Departments] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Departments");
            throw;
        }
    }

    private async Task SeedTeachersAsync()
    {
        if (await _context.Teachers.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<TeacherSeedDto>>("teachers.json");
        if (dtos == null) return;

        var entities = new List<Teacher>();
        foreach (var dto in dtos)
        {
            var entity = Teacher.CreateNew(dto.FullName, dto.DepartmentId,
                dto.PositionId, dto.UserId);
            SetPrivateProperty(entity, "Id", dto.Id);
            entities.Add(entity);
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Teachers] ON;");

            await _context.Teachers.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Teachers] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Teachers");
            throw;
        }
    }

    private async Task SeedStudentsAsync()
    {
        if (await _context.Students.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<StudentSeedDto>>("students.json");
        if (dtos == null) return;

        var entities = new List<Student>();
        foreach (var dto in dtos)
        {
            var entity = Student.CreateNew(dto.FullName, dto.Group,
                dto.YearOfEntry, dto.UserId);
            SetPrivateProperty(entity, "Id", dto.Id);
            entities.Add(entity);
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Students] ON;");

            await _context.Students.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Students] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Students");
            throw;
        }
    }

    private async Task UpdateDepartmentHeadsAsync()
    {
        var dtos =
            await LoadJsonAsync<List<DepartmentSeedDto>>("departments.json");
        if (dtos == null) return;

        // Беремо тільки ті, де HeadTeacherId вказаний в JSON
        var departmentsToUpdate =
            dtos.Where(d => d.HeadTeacherId.HasValue).ToList();
        if (departmentsToUpdate.Count == 0) return;

        var departmentIds = departmentsToUpdate.Select(d => d.Id);
        var departmentsFromDb = await _context.Departments
            .Where(d => departmentIds.Contains(d.Id))
            .ToListAsync();

        foreach (var department in departmentsFromDb)
        {
            if (department.HeadTeacherId.HasValue) continue;

            var dto = departmentsToUpdate.First(d => d.Id == department.Id);
            department.SetHead(dto.HeadTeacherId.Value);
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedTeacherSubjectsAsync()
    {
        if (await _context.Set<TeacherSubject>().AnyAsync()) return;

        var dtos = await LoadJsonAsync<List<TeacherSubjectSeedDto>>("teachersubjects.json");
        if (dtos == null) return;

        _context.ChangeTracker.Clear();

        var duplicates = dtos
            .GroupBy(d => new { d.TeacherId, d.SubjectId })
            .Where(g => g.Count() > 1)
            .ToList();

        if (duplicates.Count != 0)
        {
            _logger.LogWarning("Duplicate TeacherSubject pairs found: {Count}", duplicates.Count);
            foreach (var dup in duplicates)
            {
                _logger.LogWarning("Duplicate: TeacherId={TeacherId}, SubjectId={SubjectId}",
                    dup.Key.TeacherId, dup.Key.SubjectId);
            }
        }

        var entities = dtos
            .DistinctBy(dto => new { dto.TeacherId, dto.SubjectId })
            .Select(dto => new TeacherSubject(dto.TeacherId, dto.SubjectId));

        await _context.Set<TeacherSubject>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();
    }


    private async Task SeedStudentSubjectsAsync()
    {
        if (await _context.Set<StudentSubject>().AnyAsync()) return;
        var dtos =
            await LoadJsonAsync<List<StudentSubjectSeedDto>>(
                "studentsubjects.json");
        if (dtos == null) return;

        var entities = dtos.Select(dto =>
            new StudentSubject(dto.StudentId, dto.SubjectId));
        await _context.Set<StudentSubject>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
    private async Task SeedGradesAsync()
    {
        if (await _context.Grades.AnyAsync()) return;
        var dtos = await LoadJsonAsync<List<GradeSeedDto>>("grades.json");
        if (dtos == null) return;

        var entities = new List<Grade>();
        var currentId = 1;

        foreach (var dto in dtos)
        {
            var entity = Grade.CreateNew(dto.StudentId, dto.SubjectId,
                dto.TeacherId, dto.NumericValue, dto.LetterValue);
            SetPrivateProperty(entity, "Id", currentId++);
            entities.Add(entity);
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Grades] ON;");

            await _context.Grades.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Grades] OFF;");

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to seed Grades");
            throw;
        }
    }

    private async Task<T?> LoadJsonAsync<T>(string fileName) where T : class
    {
        var filePath = Path.Combine(_seedDataPath, fileName);
        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Seed file not found: {FileName}", fileName);
            return null;
        }

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private void SetPrivateProperty(object obj, string propertyName,
        object value)
    {
        var property = obj.GetType().GetProperty(propertyName,
            BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            throw new MissingMemberException(obj.GetType().Name, propertyName);
        }

        if (!property.CanWrite)
        {
            var setter = property.GetSetMethod(nonPublic: true);
            if (setter == null)
            {
                throw new InvalidOperationException(
                    $"Property '{propertyName}' on '{obj.GetType().Name}' has no set.");
            }

            setter.Invoke(obj, [value]);
        }
        else
        {
            property.SetValue(obj, value);
        }
    }
}