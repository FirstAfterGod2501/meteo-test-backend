using MeteoService.API.Core.Entities;
using MeteoService.API.Core.Interfaces;
using MeteoService.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeteoService.API.Infrastructure.Repositories;

/// <summary>
/// Repository for accessing weather data from the database.
/// </summary>
public class WeatherRepository : IWeatherRepository
{
    private readonly ApplicationContext _context;

    private readonly IWeatherForecastingService _forecastingService;

    private readonly ILogger<WeatherRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used by the repository.</param>
    /// <param name="forecastingService">The service for forecasting weather</param>
    /// <param name="logger">logger</param>
    public WeatherRepository(ApplicationContext context, IWeatherForecastingService forecastingService, ILogger<WeatherRepository> logger)
    {
        _context = context;
        _forecastingService = forecastingService;
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves weather data for a specific location by its unique identifier.
    /// </summary>
    /// <param name="locationId">The unique identifier for the location.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the WeatherData associated with the specified location identifier.</returns>
    public async Task<WeatherData?> GetWeatherDataAsync(Guid locationId)
    {
        return await _context.WeatherData.FindAsync(locationId);
    }

    /// <summary>
    /// Asynchronously retrieves weather data for a specific location based on its geographical coordinates and calculates a forecast using linear regression.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the forecasted WeatherData for the specified coordinates.</returns>
    public async Task<WeatherData?> GetWeatherDataByCoordinatesAsync(double latitude, double longitude)
    {
        _logger.LogInformation("Retrieving weather data for latitude: {latitude} longitude: {locationId}", 
            latitude, longitude);

        var nearestWeatherDataPoints = _context.WeatherData
            .OrderBy(w => Math.Abs(w.Latitude - latitude) + Math.Abs(w.Longitude - longitude))
            .Take(10)
            .ToList();

        //TODO: Call to another service
        if (nearestWeatherDataPoints.Count == 0)
        {
            var randomTemperature = new Random().Next(-30, 40);
            var newWeatherData = new WeatherData
            {
                Latitude = latitude,
                Longitude = longitude,
                Temperature = randomTemperature,
                WindDirection = WindDirection.East,
                Timestamp = DateTime.UtcNow,
                WindSpeed = 5
            };

            _context.WeatherData.Add(newWeatherData);
            await _context.SaveChangesAsync();
            return newWeatherData;
        }

        var forecastedTemperature = await Task.Run(() =>
            _forecastingService.CalculateForecastedTemperature(nearestWeatherDataPoints, latitude, longitude));


        // Returning the forecasted weather data
        var forecastedWeatherData = new WeatherData
        {
            Latitude = latitude,
            Longitude = longitude,
            Temperature = forecastedTemperature
        };

        return forecastedWeatherData;
    }
}