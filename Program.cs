using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagementService.Src.Data;
using UserManagementService.Src.Helpers;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Implements;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services;
using UserManagementService.Src.Services.Implements;
using UserManagementService.Src.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));
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
builder.Services.AddScoped<ICareersRepository, CareersRepository>();
builder.Services.AddScoped<IAccessManagementService, AccessManagementService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<User>, Microsoft.AspNetCore.Identity.PasswordHasher<User>>();
builder.Services.AddSingleton<RabbitMQProducer>();
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<ServerCallContextInterceptor>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        string? jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("The JWT Key cannot be null or empty.");
        }
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {

                var endpoint = context.HttpContext.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null)
                {
                    return Task.CompletedTask;
                }

                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    var token = context.Request.Headers["Authorization"];
                    Console.WriteLine($"Middleware JWT Token recibido: {token}");
                    context.Token = token.ToString().Replace("Bearer ", "");
                }
                else
                {
                    Console.WriteLine("Middleware JWT: No se encontró el encabezado 'Authorization'.");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Middleware JWT Error: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
