using MeteoService.API.API.Extensions.Vault;
using MeteoService.API.API.Middlewares;
using MeteoService.API.Core.Interfaces;
using MeteoService.API.Core.Services;
using MeteoService.API.Infrastructure.Data;
using MeteoService.API.Infrastructure.HealthChecks;
using MeteoService.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeteoService.API;

/// <summary>
/// Main entry point for the MeteoService API.
/// </summary>
public class Program
{
    /// <summary>
    /// Configures and starts the web application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add configuration from Vault
        builder.Configuration.AddVault(_ => { });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddControllers();

        // Configure PostgreSQL database context
        builder.Services.AddDbContext<ApplicationContext>(
            options =>
            {
                options.UseNpgsql(
                    $"" +
                    $"Host={builder.Configuration["POSTGRES_HOST"]};" +
                    $"Port={builder.Configuration["POSTGRES_PORT"]};" +
                    $"Database={builder.Configuration["POSTGRES_DATABASE"]};" +
                    $"Username={builder.Configuration["POSTGRES_USER"]};" +
                    $"Password={builder.Configuration["POSTGRES_PASSWORD"]}");
                options.EnableSensitiveDataLogging();
                options.EnableThreadSafetyChecks();
                options.EnableDetailedErrors();
            });

        // Add services for API exploration and Swagger documentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
        builder.Services.AddScoped<IWeatherForecastingService, WeatherForecastingService>();

        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck<PerformanceHealthCheck>("performance_health_check");

        var app = builder.Build();

        app.UseMiddleware<ResponseTimeMiddleware>();

        app.UseExceptionHandler("/error"); // Add a route for handling exceptions

        app.UseHealthChecks("/health"); // Add health checks

        app.UseRouting();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            dbContext.Database.Migrate();
        }

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.Run();
    }
}