using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Application.Auth;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Repositories;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;
using WebApi.Config;
using WebApi.Application.Projects;
using WebApi.Application.TaskItems;
using WebApi.Application.Users;
using WebApi.Serialization;

namespace WebApi.Extensions;

using Microsoft.EntityFrameworkCore;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtConfig = configuration
            .GetSection("JwtSettings")
            .Get<JwtConfiguration>()?? throw new ApplicationException(
            "Missing jwt configuration");
        if (string.IsNullOrWhiteSpace(jwtConfig.SecretKey))
        {
            throw new InvalidOperationException("JWT secret is missing or empty.");
        }
        
        services.AddSingleton(jwtConfig);
        
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Database connection is missing.");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskItemService, TaskItemService>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddApiConfiguration(
        this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(new LowerCaseNamingPolicy())
            );
        });

        return services;
    }
}