using MongoDB.Bson.Serialization.Attributes;

namespace HumoApp.Models
{
    [BsonIgnoreExtraElements]
    public class AnalysisSignals
    {
        [BsonElement("promesasLaborales")]
        public int PromesasLaborales { get; set; }

        [BsonElement("lenguajeExagerado")]
        public int LenguajeExagerado { get; set; }

        [BsonElement("faltaTransparencia")]
        public int FaltaTransparencia { get; set; }

        [BsonElement("autoridadDudosa")]
        public int AutoridadDudosa { get; set; }
    }
}
