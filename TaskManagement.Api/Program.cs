using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Configurations;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var kafkaSettings = builder.Configuration.GetSection("KafkaSettings").Get<KafkaSettings>();
//builder.Services.AddSingleton(kafkaSettings);

// Configuración de JWT desde la clase JwtConfig
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configuración de Swagger desde la clase SwaggerConfig
builder.Services.AddSwaggerDocumentation();

// Configuración de CORS desde la clase CorsConfig
builder.Services.AddCorsConfiguration();

// Configurar las dependencias (repositorios y servicios)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserServiceHandler>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

//builder.Services.AddHostedService<KafkaConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
