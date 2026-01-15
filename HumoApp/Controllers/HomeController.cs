using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HumoApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HumoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Home/TestMongo")]
        public async Task<IActionResult> TestMongo()
        {
            var conn = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            var dbName = Environment.GetEnvironmentVariable("MONGO_DATABASE");

            if (string.IsNullOrWhiteSpace(conn) || string.IsNullOrWhiteSpace(dbName))
            {
                return Json(new { ok = false, error = "MONGO_CONNECTION_STRING o MONGO_DATABASE no configurados" });
            }

            try
            {
                var client = new MongoClient(conn);
                var db = client.GetDatabase(dbName);

                var ping = await db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));

                using var cursor = await db.ListCollectionNamesAsync();
                var collections = await cursor.ToListAsync();

                return Json(new
                {
                    ok = true,
                    message = "Conexión a MongoDB OK",
                    ping = ping.ToString(),
                    collections = collections
                });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }

        [HttpPost("Home/AnalyzeOferta")]
        public IActionResult AnalyzeOferta([FromBody] OfertaAnalysisRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { ok = false, error = "Datos de oferta no proporcionados" });
            }

            //lógica de análisis de oferta...
            var result = new
            {
                ok = true,
                id = "650ef... (ObjectId)",
                url = "https://ejemplo.com/oferta/123",
                category = "Oferta-laboral",
                score = 72,
                signals = new
                {
                    promesasLaborales = 30,
                    lenguajeExagerado = 20,
                    faltaTransparencia = 12,
                    autoridadDudosa = 10
                },
                explanation = "Se detectaron frases como 'triplica tu salario' y 'plaza exclusiva' que aumentan promesas y lenguaje exagerado...",
                raw = new { llm = "texto crudo o estructura del LLM" },
                createdAt = "2026-01-15T12:34:56Z",
                modelVersion = "py-llm-v1"
            };

            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class OfertaAnalysisRequest
    {
        public string TextoOferta { get; set; }
    }
}
