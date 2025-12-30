using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.Services.Weather;

namespace NetBootcamp.API.Weather;

public class WeatherController(IWeatherService weatherService) : CustomBaseController
{
    [HttpGet]
    public IActionResult Get(string city)
    {
        var result = weatherService.GetWeatherForecasts(city);
        return CreateActionResult(result);
    }
}