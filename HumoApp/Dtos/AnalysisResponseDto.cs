using HumoApp.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace HumoApp.Dtos
{
    public class AnalysisResponseDto
    {
        public string Id { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Score { get; set; }

        [BsonElement("signals")]
        public AnalysisSignals Signals { get; set; } = new();

        public string Explanation { get; set; } = null!;

        [BsonElement("risk_level")]
        public string RiskLevel { get; set; } = null!;

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}