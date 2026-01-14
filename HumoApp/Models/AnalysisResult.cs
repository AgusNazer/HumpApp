namespace HumoApp.Models
{
    public class AnalysisResult
    {
        public string Category { get; set; } = string.Empty;
        public int Score { get; set; }
        public Signals Signals { get; set; } = new();
        public string Explanation { get; set; } = string.Empty;
    }
}
