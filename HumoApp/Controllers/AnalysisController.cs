using Microsoft.AspNetCore.Mvc;
using HumoApp.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HumoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        // POST api/<AnalysisControllercs>
        [HttpPost]
        public IActionResult Analyze([FromBody] AnalysisRequestDto analysis)
        {
            if (analysis == null)
                return BadRequest("Datos inválidos");

            //acá después va la lógica real de análisis
            return Ok(analysis);
        }
    }
}
