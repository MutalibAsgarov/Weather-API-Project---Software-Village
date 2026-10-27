using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Weather_API.DTOs
{
    public class WeatherRequestDto
    {
        [Required]
        public string City { get; set; } = string.Empty;
    }
}