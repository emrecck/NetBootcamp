using NetBootcamp.Services.SharedDTOs;

namespace NetBootcamp.Services.Weather;

public interface IWeatherService
{
    ResponseModelDto<int> GetWeatherForecasts(string city);
}
