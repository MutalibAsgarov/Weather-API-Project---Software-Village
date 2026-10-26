namespace Weather_API.DTOs
{
    public class ForecastResponseDto
    {
        public string DayName { get; set; } = string.Empty;
        public double? MinTemp { get; set; }
        public double? MaxTemp { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
    }
}
