using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HumoApp.Models
{
    public class Analysis
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        // Input
        public string Url { get; set; } = null!;

        // Resultado
        public string Category { get; set; } = null!;
        public int Score { get; set; }

        public AnalysisSignals Signals { get; set; } = new();

        public string Explanation { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
