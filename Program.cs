using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Repositories.Implements;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services.Implements;
using UserManagementService.Src.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMapperService, MapperService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
