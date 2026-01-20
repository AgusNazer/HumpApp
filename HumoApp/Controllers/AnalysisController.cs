using Microsoft.AspNetCore.Mvc;
using HumoApp.Dtos;
using HumoApp.Services;

namespace HumoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly PythonAnalysisService _pythonService;
        private readonly IMongoAnalysisService _mongoService;

        public AnalysisController(PythonAnalysisService pythonService, IMongoAnalysisService mongoService)
        {
            _pythonService = pythonService;
            _mongoService = mongoService;
        }

        [HttpPost]
        public async Task<IActionResult> Analyze([FromBody] AnalysisRequestDto analysis)
        {
            if (analysis == null || string.IsNullOrEmpty(analysis.Url))
                return BadRequest("URL inválida");

            try
            {
                // 1. Python analiza la URL
                var analysisResult = await _pythonService.AnalyzeUrl(analysis.Url);

                // 2. Convertir resultado a DTO
                var responseDto = new AnalysisResponseDto
                {
                    Url = analysis.Url,
                    Score = analysisResult.Score,
                    Signals = new(),
                    RiskLevel = analysisResult.RiskLevel
                };

                // 3. Guardar en MongoDB
                await _mongoService.SaveAnalysisAsync(responseDto);

                return Ok(responseDto);
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