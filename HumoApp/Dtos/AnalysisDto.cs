namespace HumoApp.Dtos
{
    public class AnalysisDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public DateTime CreatedAt { get; set; }
        public AnalysisResultDto Result { get; set; } = new();
    }
}
