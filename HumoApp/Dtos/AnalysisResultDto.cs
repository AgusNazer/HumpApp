namespace HumoApp.Dtos
{
    public class AnalysisResultDto
    {
        public string Category { get; set; } = string.Empty;
        public int Score { get; set; }
        public SignalsDto Signals { get; set; } = new();
        public string Explanation { get; set; } = string.Empty;
    }
}
