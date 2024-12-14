using Microsoft.EntityFrameworkCore;
using UserManagementService.Src.Data;
using UserManagementService.Src.Helpers;
using UserManagementService.Src.Repositories.Implements;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services;
using UserManagementService.Src.Services.Implements;
using UserManagementService.Src.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMapperService, MapperService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DataContext>();

    dbContext.Database.Migrate();
    await Seeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<UserServiceImpl>();
app.MapGet("/", () => "User Management Service");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
