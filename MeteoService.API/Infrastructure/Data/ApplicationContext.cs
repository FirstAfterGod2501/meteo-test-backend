namespace MeteoService.API.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Core.Entities;

/// <summary>
/// Application context for database interactions, inheriting from DbContext.
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApplicationContext with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for WeatherData entities.
    /// </summary>
    public virtual DbSet<WeatherData> WeatherData { get; set; }    
    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in DbSet properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure index for WeatherData entity based on Latitude and Longitude.
        modelBuilder.Entity<WeatherData>()
            .HasIndex(p => new { p.Latitude, p.Longitude })
            .HasDatabaseName("IX_WeatherData_Latitude_Longitude");

        // Configure index for WeatherData entity based on Timestamp.
        modelBuilder.Entity<WeatherData>()
            .HasIndex(p => p.Timestamp)
            .HasDatabaseName("IX_WeatherData_Timestamp");
    }
}