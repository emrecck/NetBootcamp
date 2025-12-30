using NetBootcamp.Services.SharedDTOs;

namespace NetBootcamp.Services.Weather;

public class WeatherService : IWeatherService
{
    public ResponseModelDto<int> GetWeatherForecasts(string city)
    {
        return ResponseModelDto<int>.Success(25);
    }
}
