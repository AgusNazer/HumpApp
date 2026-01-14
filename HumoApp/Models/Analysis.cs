namespace HumoApp.Models
{
    /// <summary>
    /// Representa un análisis realizado sobre un curso (o URL).
    /// Contiene metadata sobre cuándo y sobre qué curso se ejecutó el análisis,
    /// además del resultado del mismo.
    /// </summary>
    public class Analysis
    {
        /// <summary>
        /// Identificador único del análisis.
        /// Se genera por defecto con <see cref="Guid.NewGuid"/>.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identificador del curso asociado a este análisis.
        /// Corresponde a <see cref="Course.Id"/> del curso analizado.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Fecha y hora UTC en la que se creó el análisis.
        /// Se inicializa por defecto con <see cref="DateTime.UtcNow"/>.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Resultado del análisis con categoría, puntuación, señales y explicación.
        /// </summary>
        public AnalysisResult Result { get; set; } = new();
    }
}
