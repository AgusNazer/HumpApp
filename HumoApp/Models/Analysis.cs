using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HumoApp.Models;

public class Analysis
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    // Entrada
    public string Url { get; set; } = string.Empty;

    // Resultado
    public string Category { get; set; } = string.Empty;
    public int Score { get; set; }

    public AnalysisSignals Signals { get; set; } = new();

    public string Explanation { get; set; } = string.Empty;

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
