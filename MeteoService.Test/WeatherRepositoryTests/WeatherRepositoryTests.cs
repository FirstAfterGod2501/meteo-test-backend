using MeteoService.API.Core.Entities;
using MeteoService.API.Core.Interfaces;
using MeteoService.API.Infrastructure.Data;
using MeteoService.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeteoService.Test.WeatherRepositoryTests;

[TestFixture]
public class WeatherRepositoryTests
{
    private Mock<ApplicationContext> _mockContext;
    private Mock<IWeatherForecastingService> _mockForecastingService;
    private WeatherRepository _weatherRepository;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<ApplicationContext>();
        _mockForecastingService = new Mock<IWeatherForecastingService>();

        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        _mockContext = new Mock<ApplicationContext>(options);
        
        var mockLogger = new Mock<ILogger<WeatherRepository>>();

        _weatherRepository = new WeatherRepository(_mockContext.Object, _mockForecastingService.Object, mockLogger.Object);

        var weatherDataList = new List<WeatherData>
        {
            new WeatherData { Latitude = 34.05, Longitude = -118.25, Temperature = 20 },
            new WeatherData { Latitude = 34.06, Longitude = -118.24, Temperature = 22 }
        };

        var dbSetMock = new Mock<DbSet<WeatherData>>();
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.Provider).Returns(weatherDataList.AsQueryable().Provider);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.Expression).Returns(weatherDataList.AsQueryable().Expression);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.ElementType).Returns(weatherDataList.AsQueryable().ElementType);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.GetEnumerator()).Returns(() => weatherDataList.GetEnumerator());
        _mockContext.Setup(c => c.WeatherData).Returns(dbSetMock.Object);
    }

    [Test]
    public async Task GetWeatherDataAsync_ReturnsData_WhenLocationExists()
    {
        var locationId = Guid.NewGuid();
        var weatherData = new WeatherData { Id = locationId };
        var dbSetMock = new Mock<DbSet<WeatherData>>();
        dbSetMock.Setup(m => m.FindAsync(locationId)).ReturnsAsync(weatherData);
        _mockContext.Setup(c => c.WeatherData).Returns(dbSetMock.Object);

        var result = await _weatherRepository.GetWeatherDataAsync(locationId);

        Assert.That(result, Is.EqualTo(weatherData));
    }

    [Test]
    public async Task GetWeatherDataByCoordinatesAsync_ReturnsForecastedData_WhenDataExists()
    {
        double latitude = 34.05, longitude = -118.25;
        var weatherDataList = new List<WeatherData>
        {
            new WeatherData { Latitude = 34.05, Longitude = -118.25, Temperature = 20 },
            new WeatherData { Latitude = 34.06, Longitude = -118.24, Temperature = 22 }
        };
        var queryableWeatherData = weatherDataList.AsQueryable();

        var dbSetMock = new Mock<DbSet<WeatherData>>();
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.Provider).Returns(queryableWeatherData.Provider);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.Expression).Returns(queryableWeatherData.Expression);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.ElementType).Returns(queryableWeatherData.ElementType);
        dbSetMock.As<IQueryable<WeatherData>>().Setup(m => m.GetEnumerator()).Returns(() => queryableWeatherData.GetEnumerator());

        _mockContext.Setup(c => c.WeatherData).Returns(dbSetMock.Object);

        _mockForecastingService.Setup(f => f.CalculateForecastedTemperature(It.IsAny<List<WeatherData>>(), latitude, longitude))
            .Returns(21); 

        var result = await _weatherRepository.GetWeatherDataByCoordinatesAsync(latitude, longitude);

        Assert.IsNotNull(result);
        Assert.That(result.Temperature, Is.EqualTo(21));
    }
}