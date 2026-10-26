using Weather_API.DTOs;

namespace Weather_API.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(string city);
        Task<List<ForecastResponseDto>> GetForecastAsync(string city);
        Task<List<RainProbabilityResponseDto>> GetRainProbabilityAsync(string city);

    }
}