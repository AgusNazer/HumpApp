namespace HumoApp.Dtos
{
    public class AnalysisResultDto
    {
        public double Score { get; set; }
        public string Verdict { get; set; }
        public List<string> RedFlags { get; set; }
    }
}
