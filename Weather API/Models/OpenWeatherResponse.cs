using System.Text.Json.Serialization;

namespace Weather_API.Models
{
    public class OpenWeatherResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new();

        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; } = new();

        [JsonPropertyName("sys")]
        public SysInfo Sys { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; } = new();

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public double Deg { get; set; }
    }

    public class SysInfo
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}