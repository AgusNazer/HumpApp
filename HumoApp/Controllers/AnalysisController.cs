using Microsoft.AspNetCore.Mvc;
using HumoApp.Dtos;
using System.Text.Json;

namespace HumoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl = "http://localhost:8000/analyze"; // Cambiar según tu URL

        public AnalysisController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

                if (analysisResult == null)
                    return BadRequest("No se pudo deserializar la respuesta");

                return Ok(analysisResult);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, new { error = "No se puede conectar con la API de Python", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno", details = ex.Message });
            }
        }
    }
}