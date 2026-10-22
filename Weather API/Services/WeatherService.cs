using System.Net.Http;
using System.Text.Json;
using Weather_API.DTOs;
using Weather_API.Models;
using Weather_API.Services.Interfaces;

namespace Weather_API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(string city)
        {
            DotNetEnv.Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("API key not found");
            }


            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("City not found");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Weather API request failed");
            }

            var content = await response.Content.ReadAsStringAsync();

            var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(content);

            if (weatherData is null)
            {
                throw new Exception("Failed to deserialize weather data");
            }

            var cityTime = TimeSpan.FromSeconds(weatherData.Timezone);

            return new WeatherResponseDto
            {
                City = weatherData.Name,
                Country = weatherData.Sys.Country,
                Temperature = weatherData.Main.Temp,
                RealFeel = weatherData.Main.FeelsLike,
                WindSpeed = weatherData.Wind.Speed,
                WindDirection = GetWindDirection(weatherData.Wind.Deg),
                Pressure = weatherData.Main.Pressure,
                Humidity = weatherData.Main.Humidity,
                Sunrise = TimeOnly.FromDateTime(
                    DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunrise)
                        .ToOffset(cityTime).DateTime),
                Sunset = TimeOnly.FromDateTime(
                    DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunset)
                        .ToOffset(cityTime).DateTime),
                Icon = weatherData.Weather.FirstOrDefault()?.Icon ?? string.Empty,
                Description = weatherData.Weather.FirstOrDefault()?.Description ?? string.Empty,
                DateTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.Dt)
                    .ToOffset(cityTime).DateTime
            };
        }

        private static string GetWindDirection(double degrees)
        {
            string[] directions =
            {
                "N", "NE", "E", "SE",
                "S", "SW", "W", "NW"
            };

            var index = (int)Math.Round(degrees / 45) % 8;

            return directions[index];
        }
    }
}