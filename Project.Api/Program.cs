using Microsoft.OpenApi.Models;
using Application.Validation.User;
using Application.Validation.Auth;
using Application.Validation.Report;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Validation.Grade;
using Application.Validation.Student;
using Application.Validation.Subject;
using Application.Validation.Teacher;
using Infrastructure.Persistence;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Filters;
using Project.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add(new ApiExceptionFilter()); });

// отключение стандартной обработки ошибки отсутствия полей в ДТО/ошибки валидации.
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AssignSubjectStudentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddGradeDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateGradeDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetGradesQueryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CalculateStudentGpaQueryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetGroupStatisticsQueryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetGroupReportQueryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddSubjectDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateSubjectDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AssignSubjectTeacherDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UnassignSubjectTeacherDtoValidator>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Сід для БД
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         if ((await context.Database.GetPendingMigrationsAsync()).Any())
//         {
//             await context.Database.MigrateAsync();
//         }
//
//         var seeder = services.GetRequiredService<DataSeeder>();
//         await seeder.Seed();
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "Error seeding bd.");
//     }
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // MIDDLEWARE
app.UseMiddleware<JwtTokenVersionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();