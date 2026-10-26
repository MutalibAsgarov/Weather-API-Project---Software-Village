
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
            var apiKey = GetApiKey();

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

        public async Task<List<ForecastResponseDto>> GetForecastAsync(string city)
        {
            var apiKey = GetApiKey();

            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";

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

            using var document = JsonDocument.Parse(content);

            var root = document.RootElement;
            var timezone = root.GetProperty("city").GetProperty("timezone").GetInt32();

            var items = root.GetProperty("list").EnumerateArray()
                .Select(item =>
                {
                    var timestamp = item.GetProperty("dt").GetInt64();

                    var localTime = DateTimeOffset
                        .FromUnixTimeSeconds(timestamp)
                        .ToOffset(TimeSpan.FromSeconds(timezone))
                        .DateTime;

                    return new
                    {
                        Date = localTime.Date,
                        Item = item
                    };
                })
                .ToList();

            var firstDate = DateTimeOffset.UtcNow
                .ToOffset(TimeSpan.FromSeconds(timezone))
                .DateTime.Date;

            var result = new List<ForecastResponseDto>();

            for (var i = 0; i < 7; i++)
            {
                var date = firstDate.AddDays(i);
                var dayItems = items.Where(x => x.Date == date).ToList();

                if (dayItems.Count == 0)
                {
                    result.Add(new ForecastResponseDto
                    {
                        DayName = date.ToString("ddd").ToUpperInvariant(),
                        MinTemp = null,
                        MaxTemp = null,
                        Icon = null,
                        Description = null
                    });

                    continue;
                }

                var temperatures = dayItems
                    .Select(x => x.Item.GetProperty("main"));

                var weather = dayItems[0].Item.GetProperty("weather")[0];

                result.Add(new ForecastResponseDto
                {
                    DayName = date.ToString("ddd").ToUpperInvariant(),
                    MinTemp = Math.Round(
                        temperatures.Min(x => x.GetProperty("temp_min").GetDouble()), 1),
                    MaxTemp = Math.Round(
                        temperatures.Max(x => x.GetProperty("temp_max").GetDouble()), 1),
                    Icon = weather.GetProperty("icon").GetString(),
                    Description = weather.GetProperty("description").GetString()
                });
            }

            return result;
        }

        public async Task<List<RainProbabilityResponseDto>> GetRainProbabilityAsync(string city)
        {
            var apiKey = GetApiKey();

            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";

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

            using var document = JsonDocument.Parse(content);

            var root = document.RootElement;
            var timezone = root.GetProperty("city").GetProperty("timezone").GetInt32();

            var forecasts = root.GetProperty("list").EnumerateArray()
                .Select(item =>
                {
                    var timestamp = item.GetProperty("dt").GetInt64();

                    var localTime = DateTimeOffset
                        .FromUnixTimeSeconds(timestamp)
                        .ToOffset(TimeSpan.FromSeconds(timezone))
                        .DateTime;

                    return new
                    {
                        Time = localTime,
                        Item = item
                    };
                })
                .ToList();

            var now = DateTimeOffset.UtcNow
                .ToOffset(TimeSpan.FromSeconds(timezone))
                .DateTime;

            var start = new DateTime(
                now.Year, now.Month, now.Day, now.Hour, 0, 0
            ).AddHours(1);

            var result = new List<RainProbabilityResponseDto>();

            for (var i = 0; i < 6; i++)
            {
                var hour = start.AddHours(i);

                var forecast = forecasts.FirstOrDefault(x =>
                    x.Time.Date == hour.Date &&
                    x.Time.Hour == hour.Hour);

                int? probability = null;
                string? condition = null;

                if (forecast != null)
                {
                    probability = (int)Math.Round(
                        forecast.Item.GetProperty("pop").GetDouble() * 100);

                    var weather = forecast.Item.GetProperty("weather")[0];
                    var main = weather.GetProperty("main").GetString();

                    condition = main switch
                    {
                        "Thunderstorm" => "Heavy",
                        "Rain" or "Drizzle" => "Rainy",
                        "Clear" => "Sunny",
                        "Clouds" => "Cloudy",
                        _ => null
                    };
                }

                result.Add(new RainProbabilityResponseDto
                {
                    TimeLabel = hour.ToString("htt").ToUpperInvariant(),
                    Probability = probability,
                    Condition = condition
                });
            }

            return result;
        }

        private static string GetApiKey()
        {
            DotNetEnv.Env.Load();

            var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("API key not found");
            }

            return apiKey;
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
