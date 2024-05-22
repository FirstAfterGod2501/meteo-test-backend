namespace MeteoService.API.Core.Entities;

/// <summary>
/// Represents the data structure used for storing distance and temperature related calculations
/// necessary for linear regression analysis in weather forecasting.
/// </summary>
public struct DistanceTemperatureData
{
    /// <summary>
    /// Represents the distance from a given point to the weather data point.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Represents the temperature at the weather data point.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Represents the square of the distance (X squared).
    /// </summary>
    public double Xx { get; set; }

    /// <summary>
    /// Represents the product of distance and temperature (X multiplied by Y).
    /// </summary>
    public double Xy { get; set; }
}