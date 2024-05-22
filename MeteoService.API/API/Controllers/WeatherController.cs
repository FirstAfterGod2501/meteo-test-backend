using MeteoService.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeteoService.API.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherRepository _weatherRepository;

    public WeatherController(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather(double latitude, double longitude)
    {
        var weatherData = await _weatherRepository.GetWeatherDataByCoordinatesAsync(latitude, longitude);
        if (weatherData == null)
            return NotFound();

        return Ok(weatherData);
    }
}