using Weather_API.DTOs;

namespace Weather_API.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(string city);
    }
}