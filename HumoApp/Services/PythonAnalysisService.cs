namespace HumoApp.Services
{
    public class PythonAnalysisService
    {
        private readonly HttpClient _httpClient;

        public PythonAnalysisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8000"); // o el puerto que uses
        }

        public async Task<AnalysisResult> AnalyzeUrl(string url)
        {
            var response = await _httpClient.PostAsJsonAsync("/analyze", new { url = url });
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AnalysisResult>();
        }
    }
    public class AnalysisResult
    {
        public int Score { get; set; }
        public Dictionary<string, int> Signals { get; set; }
        public string RiskLevel { get; set; }
    }
}
