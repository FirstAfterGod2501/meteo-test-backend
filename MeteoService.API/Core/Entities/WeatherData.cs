namespace MeteoService.API.Core.Entities;

/// <summary>
/// Represents the weather data for a specific location and time.
/// </summary>
public class WeatherData
{
    /// <summary>
    /// Unique identifier for the weather data entry.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Latitude of the location for which the weather data is recorded.
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Longitude of the location for which the weather data is recorded.
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Temperature at the specified location and time.
    /// </summary>
    public double Temperature { get; set; }
    
    /// <summary>
    /// Wind speed at the specified location and time.
    /// </summary>
    public double WindSpeed { get; set; }
    
    /// <summary>
    /// Wind direction at the specified location and time.
    /// </summary>
    public WindDirection WindDirection { get; set; }
    
    /// <summary>
    /// Timestamp when the weather data was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
