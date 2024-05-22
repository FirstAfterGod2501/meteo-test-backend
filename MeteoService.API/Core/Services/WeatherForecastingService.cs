using MeteoService.API.Core.Entities;
using MeteoService.API.Core.Interfaces;

namespace MeteoService.API.Core.Services;

/// <summary>
/// Provides weather forecasting services using the Least Squares method for linear regression.
/// </summary>
public class WeatherForecastingService : IWeatherForecastingService
{
    /// <summary>
    /// Calculates the forecasted temperature for a specific location based on historical weather data.
    /// </summary>
    /// <param name="weatherDataPoints">A list of historical weather data points.</param>
    /// <param name="latitude">The latitude of the location for which the forecast is to be calculated.</param>
    /// <param name="longitude">The longitude of the location for which the forecast is to be calculated.</param>
    /// <returns>The forecasted temperature at the specified location.</returns>
    public double CalculateForecastedTemperature(List<WeatherData> weatherDataPoints, double latitude,
        double longitude)
    {
        var results = CalculateDistanceAndTemperature(weatherDataPoints, latitude, longitude);
        return CalculateLinearRegression(results, weatherDataPoints.Count);
    }

    /// <summary>
    /// Calculates the distance and temperature data needed for linear regression analysis.
    /// </summary>
    /// <param name="weatherDataPoints">A list of weather data points.</param>
    /// <param name="latitude">The latitude of the query point.</param>
    /// <param name="longitude">The longitude of the query point.</param>
    /// <returns>A list of distance and temperature data.</returns>
    private List<DistanceTemperatureData> CalculateDistanceAndTemperature(List<WeatherData> weatherDataPoints, double latitude, double longitude)
    {
        return weatherDataPoints.AsParallel().Select(data => {
            var latDiff = data.Latitude - latitude;
            var lonDiff = data.Longitude - longitude;
            var x = Math.Sqrt(latDiff * latDiff + lonDiff * lonDiff);
            var y = data.Temperature;

            return new DistanceTemperatureData { X = x, Y = y, Xx = x * x, Xy = x * y };
        }).ToList();
    }

    /// <summary>
    /// Performs linear regression using the Least Squares method to predict the temperature.
    /// </summary>
    /// <param name="results">A list of distance and temperature data.</param>
    /// <param name="n">The number of data points.</param>
    /// <returns>The predicted temperature at the query point.</returns>
    private double CalculateLinearRegression(List<DistanceTemperatureData> results, int n)
    {
        var sumX = results.Sum(item => item.X);
        var sumY = results.Sum(item => item.Y);
        var sumXx = results.Sum(item => item.Xx);
        var sumXy = results.Sum(item => item.Xy);

        if ((n * sumXx - sumX * sumX) == 0)
        {
            var intercept = (sumY - 0 * sumX) / n;

            return intercept + 0 * 0; 
        }
        else
        {
            var slope = (n * sumXy - sumX * sumY) / (n * sumXx - sumX * sumX);
            var intercept = (sumY - slope * sumX) / n;

            return intercept + slope * 0;
        }
    }
}
