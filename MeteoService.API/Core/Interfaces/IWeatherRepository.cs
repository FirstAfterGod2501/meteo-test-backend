using MeteoService.API.Core.Entities;

namespace MeteoService.API.Core.Interfaces;

/// <summary>
/// Provides an interface for accessing weather data from a data repository.
/// </summary>
public interface IWeatherRepository
{
    /// <summary>
    /// Retrieves weather data for a specific location based on its unique identifier.
    /// </summary>
    /// <param name="locationId">The unique identifier for the location.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the WeatherData associated with the specified location identifier.</returns>
    Task<WeatherData?> GetWeatherDataAsync(Guid locationId);

    /// <summary>
    /// Retrieves weather data for a specific location based on its geographical coordinates.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the WeatherData associated with the specified coordinates.</returns>
    Task<WeatherData?> GetWeatherDataByCoordinatesAsync(double latitude, double longitude);
}