using HumoApp.Models;

namespace HumoApp.Dtos
{
    public class AnalyzeResponseDto
    {
        public string Id { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Score { get; set; }

        public AnalysisSignals Signals { get; set; } = new();

        public string Explanation { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
