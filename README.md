HumoApp — API .NET (MVP)
Resumen

Backend principal en .NET 10 (C# 14).
Responsabilidad: exponer la API, orquestar llamadas al microservicio Python (análisis LLM), y persistir/servir resultados desde MongoDB.
MVP: recibir una URL, delegar el análisis a Python, almacenar el resultado y exponerlo vía API.
Arquitectura (breve)

Frontend (externo) → Backend .NET (esta API) → Microservicio Python (LLM) → MongoDB.
Decisión de MVP: el servicio Python realiza el análisis y persiste el documento Analysis en Mongo; .NET consume la respuesta y expone endpoints para la UI.
Requisitos

.NET 10 SDK
MongoDB (local o hosted)
Servicio Python con endpoint POST /api/analyze que persista en Mongo y devuelva id
Paquetes NuGet recomendados: MongoDB.Driver, DotNetEnv
Variables de entorno (ej. .env)

MONGO_CONNECTION_STRING — cadena de conexión a MongoDB.
MONGO_DATABASE — nombre de la base de datos.
PYTHON_API_URL — URL base del servicio Python (ej.: http://localhost:8000).
PYTHON_API_KEY — (opcional) API key para autenticar con Python.
Modelo de datos (resumen)

Documento Analysis (colección analyses) con campos principales:
Id (ObjectId como string)
Url
Category
Score (int)
Signals (objeto: promesasLaborales, lenguajeExagerado, faltaTransparencia, autoridadDudosa)
Explanation (string)
CreatedAt (ISO)
ModelVersion (string)
Raw (opcional, salida cruda del LLM)
Endpoints principales (API .NET)

POST /api/analysis

Descripción: envía url al servicio Python; espera respuesta (Python persiste en Mongo) y devuelve la representación guardada.
Request (JSON): { "url": "https://ejemplo.com/oferta/123", "requestId": "opcional-uuid", "metadata": { "language": "es" } }
Response (200 OK): objeto Analysis con id.
Códigos: 200 (OK), 400 (input inválido), 502/503 (error con Python), 500 (error interno).
GET /api/analysis/{id}

Descripción: obtiene resultado por id (ObjectId).
Response: objeto Analysis o 404 si no existe.
Contrato esperado con la API Python (resumen)

Endpoint Python: POST /api/analyze
Payload esperado (mínimo): { "url": "...", // opcional si envías "html" "html": null, // opcional "requestId": "...", // opcional para correlación "metadata": {...} // opcional }
Comportamiento Python:
validar input, obtener/normalizar contenido si hace falta,
ejecutar análisis LLM,
construir documento Analysis,
persistir en Mongo (colección analyses),
devolver JSON con al menos: { "ok": true, "id": "", "url": "...", "category": "...", "score": 72, "signals": { "promesasLaborales": 30, "lenguajeExagerado": 20, ... }, "explanation": "...", "createdAt": "2026-01-15T12:34:56Z", "modelVersion": "py-llm-v1", "raw": { /* opcional */ } }
En errores: devolver formato uniforme: { "ok": false, "error": { "code": "STRING", "message": "..." } }
Ejemplos (curl)

Solicitar análisis: curl -X POST http://localhost:5000/api/analysis
-H "Content-Type: application/json"
-d '{"url":"https://ejemplo.com/oferta/123"}'

Obtener resultado: curl http://localhost:5000/api/analysis/

Ejecución local (rápido)

Configura .env con MONGO_CONNECTION_STRING, MONGO_DATABASE, PYTHON_API_URL, (opcional) PYTHON_API_KEY.
Levanta MongoDB (local o Docker).
Levanta el servicio Python (FastAPI/uvicorn) y verifica POST /api/analyze.
Ejecuta la API .NET (Visual Studio 2026 o dotnet run).
Usa curl/Postman para probar POST/GET.
Buenas prácticas y recomendaciones

Seguridad: comunicar entre servicios con TLS y autenticación (API key o JWT).
Correlación: usar X-Request-Id en peticiones entre servicios.
Resiliencia: HttpClient con timeout y retry/backoff; considerar circuit-breaker.
Idempotencia: considerar búsquedas por url o hashing para evitar duplicados.
Observabilidad: logs estructurados, métricas y almacenamiento de modelVersion y raw para auditoría.
Versionado API: exponer OpenAPI/Swagger en desarrollo y versionar endpoints (v1).
Siguientes pasos recomendados (priorizados)

Implementar stub minimal en Python (FastAPI + Motor/PyMongo) que persista en Mongo y devuelva id.
Integrar PythonAnalysisService en .NET y comprobar flujo end-to-end.
Añadir pruebas de integración y docker-compose (mongo + python + dotnet) para desarrollo.
Implementar autenticación/usuarios y pruebas de seguridad antes de producción.
Archivos clave en este repositorio

Program.cs — configuración y DI.
Controllers/AnalysisController.cs — endpoints API.
Services/PythonAnalysisService.cs — cliente HTTP para Python.
Repositories/AnalysisRepository.cs — persistencia en Mongo.
Models/Analysis.cs, Models/AnalysisSignals.cs — modelos.
Notas finales

El README está pensado para el MVP. Versiona y publica el OpenAPI del servicio Python y del backend .NET antes de ampliar funcionalidades de usuarios y seguridad.
