using Microsoft.AspNetCore.Mvc;
using HumoApp.Dtos;
using HumoApp.Services;
using System.Text.Json;

namespace HumoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMongoAnalysisService _mongoService;
        private readonly string _pythonApiUrl = "http://localhost:8000/analyze"; // Cambiar según tu URL

        public AnalysisController(HttpClient httpClient, IMongoAnalysisService mongoService)
        {
            _httpClient = httpClient;
            _mongoService = mongoService;
        }

        [HttpPost]
        public async Task<IActionResult> Analyze([FromBody] AnalysisRequestDto analysis)
        {
            if (analysis == null || string.IsNullOrEmpty(analysis.Url))
                return BadRequest("URL inválida");

            try
            {
                // Preparar el request para la API de Python
                var request = new { url = analysis.Url };
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                // Llamar a la API de Python
                var response = await _httpClient.PostAsync(_pythonApiUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Error al analizar la URL en la API de Python");

                // Leer la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserializar con JsonSerializerOptions para manejar snake_case
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var analysisResult = JsonSerializer.Deserialize<AnalysisResponseDto>(
                    responseContent,
                    options
                );

                if (analysisResult != null)
                {
                    analysisResult.Url = analysis.Url;
                    // Guardar en MongoDB
                    await _mongoService.SaveAnalysisAsync(analysisResult);
                }

                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnalysis(string id)
        {
            var analysis = await _mongoService.GetAnalysisByIdAsync(id);
            if (analysis == null)
                return NotFound();
            return Ok(analysis);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnalysis()
        {
            var analyses = await _mongoService.GetAllAnalysisAsync();
            return Ok(analyses);
        }
    }
}