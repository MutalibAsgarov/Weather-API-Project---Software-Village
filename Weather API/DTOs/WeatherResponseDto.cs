namespace Weather_API.DTOs
{
    public class WeatherResponseDto
    {
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double RealFeel { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public TimeOnly Sunrise { get; set; }
        public TimeOnly Sunset { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
    }
}
