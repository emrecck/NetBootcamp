using NetBootcamp.Web.Models;
using NetBootcamp.Web.Services.Token;
using System.Net.Http.Headers;

namespace NetBootcamp.Web.Services.Weather
{
    public class WeatherService(HttpClient httpClient, TokenService tokenService, ILogger<WeatherService> logger)
    {
        public async Task<ResponseModelDto<int>> GetWeatherAsync(string city)
        {
            var response = await httpClient.GetAsync($"/api/weather?city={city}");

            if (!response.IsSuccessStatusCode)
                return ResponseModelDto<int>.Fail($"Failed to get weather data: {response.ReasonPhrase}");

            var weatherData = await response.Content.ReadFromJsonAsync<ResponseModelDto<int>>();
            if (weatherData is null)
            {
                return ResponseModelDto<int>.Fail("No weather data found");
            }
            return weatherData;

        }

        public async Task<string> GetWeatherBetterVersionAsync(string city)
        {
            var response = await httpClient.GetAsync($"/api/weather?city={city}");

            var weatherData = await response.Content.ReadFromJsonAsync<ServiceResponseModel<int>>();
            if (!response.IsSuccessStatusCode)
            {
                weatherData.FailMessages.ForEach(x => logger.LogError(x));

                return $"Sıcaklık bilgisi alınamadı.";
            }

            return weatherData.Data.ToString();

        }
    }
}
