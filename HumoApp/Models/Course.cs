namespace HumoApp.Models
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? RawContent { get; set; }
    }
}
