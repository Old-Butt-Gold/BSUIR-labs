using DistComp_1.Extensions;
using DistComp_1.Infrastructure.Mapper;
using DistComp_1.Infrastructure.Validators;
using DistComp_1.Middleware;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Infrastructure
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Регистрируем все валидаторы из сборки (текущей), где находится StoryRequestDTOValidator
// Scoped lifetime
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestDTOValidator>();

builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

// Middleware для глобальных ошибок
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();