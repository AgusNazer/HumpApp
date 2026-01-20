using System.Text;
using System.Text.Json;

namespace HumoApp.Services
{
    public class PythonAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl = "http://localhost:8000/analyze";

        public PythonAnalysisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8000"); 
        }

        public async Task<AnalysisResult> AnalyzeUrl(string url)
        {
            var jsonContent = JsonSerializer.Serialize(new { url = url });
            var response = await _httpClient.PostAsync(_pythonApiUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
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
