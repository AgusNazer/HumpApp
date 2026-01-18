using MongoDB.Bson.Serialization.Attributes;

namespace HumoApp.Models
{
    [BsonIgnoreExtraElements]
    public class SignalDetail
    {
        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("meaning")]
        public string Meaning { get; set; } = null!;
    }

    [BsonIgnoreExtraElements]
    public class AnalysisSignals
    {
        [BsonElement("promesa_empleo")]
        public SignalDetail PromesaEmpleo { get; set; } = new();

        [BsonElement("promesa_sueldo")]
        public SignalDetail PromesaSueldo { get; set; } = new();

        [BsonElement("tiempo_irreal")]
        public SignalDetail TiempoIrreal { get; set; } = new();

        [BsonElement("seniority_falso")]
        public SignalDetail SeniorityFalso { get; set; } = new();

        [BsonElement("exageracion")]
        public SignalDetail Exageracion { get; set; } = new();
    }
}