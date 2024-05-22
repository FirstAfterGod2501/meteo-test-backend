using MeteoService.API.Core.Entities;
using MeteoService.API.Core.Services;

namespace MeteoService.Test.WeatherForecastingTests;

public class WeatherForecastingTest
{
    [TestFixture]
    public class WeatherForecastingServiceTests
    {
        private WeatherForecastingService _weatherForecastingService;

        [SetUp]
        public void Setup()
        {
            _weatherForecastingService = new WeatherForecastingService();
        }

        [Test]
        public void CalculateForecastedTemperature_WithNoDataPoints_ReturnsNan()
        {
            var result = _weatherForecastingService.CalculateForecastedTemperature(new List<WeatherData>(), 0, 0);
            Assert.That(result, Is.EqualTo(Double.NaN), "Expected temperature to be Nan when no data points are provided.");
        }

        [Test]
        public void CalculateForecastedTemperature_WithSingleDataPoint_ReturnsDataPointTemperature()
        {
            var weatherDataPoints = new List<WeatherData>
            {
                new() { Latitude = 0, Longitude = 0, Temperature = 25 }
            };

            var result = _weatherForecastingService.CalculateForecastedTemperature(weatherDataPoints, 0, 0);
            Assert.That(result, Is.EqualTo(25), "Expected temperature to be the same as the single data point.");
        }

        [Test]
        public void CalculateForecastedTemperature_WithMultipleDataPoints_ReturnsCalculatedTemperature()
        {
            var weatherDataPoints = new List<WeatherData>
            {
                new() { Latitude = 0, Longitude = 0, Temperature = 20 },
                new() { Latitude = 1, Longitude = 1, Temperature = 30 },
                new() { Latitude = 2, Longitude = 2, Temperature = 25 }
            };

            var result = _weatherForecastingService.CalculateForecastedTemperature(weatherDataPoints, 1, 1);

            Assert.That(result, Is.EqualTo(30.0d), "Expected temperature to be calculated based on linear regression.");
        }
    }
}