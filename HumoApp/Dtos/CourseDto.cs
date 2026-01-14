namespace HumoApp.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Title { get; set; }
    }
}
