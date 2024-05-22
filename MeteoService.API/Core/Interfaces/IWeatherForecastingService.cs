using MeteoService.API.Core.Entities;

namespace MeteoService.API.Core.Interfaces;

/// <summary>
/// Defines the contract for a service that calculates weather forecasts.
/// </summary>
public interface IWeatherForecastingService
{
    /// <summary>
    /// Calculates the forecasted temperature for a specific location based on historical weather data.
    /// </summary>
    /// <param name="weatherDataPoints">A list of historical weather data points.</param>
    /// <param name="latitude">The latitude of the location for which the forecast is to be calculated.</param>
    /// <param name="longitude">The longitude of the location for which the forecast is to be calculated.</param>
    /// <returns>The forecasted temperature at the specified location.</returns>
    double CalculateForecastedTemperature(List<WeatherData> weatherDataPoints, double latitude, double longitude);
}