using Microsoft.AspNetCore.Mvc;
using Weather_API.DTOs;
using Weather_API.Services.Interfaces;

namespace Weather_API.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }


        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] WeatherRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.City))
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = "City is required"
                });
            }

            try
            {
                var result = await _weatherService.GetWeatherAsync(request.City);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "OK"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = ex.Message
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    success = false,
                    data = (object?)null,
                    message = "Weather API is unavailable"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An internal server error occurred"
                });
            }
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast([FromQuery] string? city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = "City is required"
                });
            }

            try
            {
                var data = await _weatherService.GetForecastAsync(city);

                return Ok(new
                {
                    success = true,
                    data,
                    message = "Forecast retrieved successfully"
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = "City not found"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An unexpected error occurred"
                });
            }
        }

        [HttpGet("rain-probability")]
        public async Task<IActionResult> GetRainProbability([FromQuery] string? city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new
                {
                    success = false,
                    data = (object?)null,
                    message = "City is required"
                });
            }

            try
            {
                var data = await _weatherService.GetRainProbabilityAsync(city);

                return Ok(new
                {
                    success = true,
                    data,
                    message = "Rain probability retrieved successfully"
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    success = false,
                    data = (object?)null,
                    message = "City not found"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An unexpected error occurred"
                });
            }
        }
    }
}