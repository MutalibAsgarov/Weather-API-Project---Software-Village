namespace Weather_API.DTOs
{
    public class RainProbabilityResponseDto
    {
        public string TimeLabel { get; set; } = string.Empty;
        public int? Probability { get; set; }
        public string Condition { get; set; } = string.Empty;
    }
}
