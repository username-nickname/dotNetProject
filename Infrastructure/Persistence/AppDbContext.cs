namespace Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<TeacherSubject> TeacherSubjects { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // таблиця ролей
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
        });

        // таблиця юзерів
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            // будет еще поле ID - для ID юзера,который присвоен ему в АПИ проекте регистрации
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.TokenVersion).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt).IsRequired();
            
            // звязок з ролями
            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // таблиця посад
        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Positions");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
        });
        
        // таблиця кафедри
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Departments");
            entity.HasKey(u => u.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt).IsRequired();
            
            entity.HasOne(d => d.HeadTeacher)
                .WithMany()
                .HasForeignKey(d => d.HeadTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // таблиця вчителів 
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teachers");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt).IsRequired();
            
            entity.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.Department).WithMany(d => d.Teachers).HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.Restrict); // Teachers - колекція в Department
            entity.HasOne(t => t.Position).WithMany().HasForeignKey(t => t.PositionId).OnDelete(DeleteBehavior.Restrict);

        });

        // таблиця предметів 
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subjects");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Semester).IsRequired();
            entity.Property(u => u.Credits).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt).IsRequired();
        });
        
        // таблиця студентів 
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Group).IsRequired().HasMaxLength(50);
            entity.Property(u => u.YearOfEntry).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt).IsRequired();
            
            entity.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
        });
        
        // таблиця оцінок 
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grades");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.LetterValue).IsRequired();
            entity.Property(u => u.NumericValue).IsRequired();
            
            entity.HasOne(g => g.Student).WithMany() .HasForeignKey(g => g.StudentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(g => g.Subject).WithMany() .HasForeignKey(g => g.SubjectId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(g => g.Teacher).WithMany() .HasForeignKey(g => g.TeacherId).OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<StudentSubject>(entity =>
        {
            entity.HasKey(ss => new { ss.StudentId, ss.SubjectId });

            entity.HasOne(ss => ss.Student)
                .WithMany(s => s.Subjects)
                .HasForeignKey(ss => ss.StudentId);

            entity.HasOne(ss => ss.Subject)
                .WithMany(s => s.Students)
                .HasForeignKey(ss => ss.SubjectId) 
                .OnDelete(DeleteBehavior.Restrict); 
        });
        
        modelBuilder.Entity<TeacherSubject>(entity =>
        {
            entity.HasKey(ss => new { ss.TeacherId, ss.SubjectId });
            entity.HasOne(ts => ts.Teacher)
                .WithMany(t => t.Subjects)
                .HasForeignKey(ts => ts.TeacherId);

            entity.HasOne(ts => ts.Subject)
                .WithMany(s => s.Teachers)
                .HasForeignKey(ts => ts.SubjectId); 
        });
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Обновление полей CreatedAt и/или UpdatedAt. Проходит по всем сушностям,которые реализуют интерфейс для отслеживания timestamps
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}