# HumoApp — API .NET (MVP)

Resumen
- Backend principal en `NET 10` (C# 14).
- Responsabilidad: exponer la API, orquestar llamadas al microservicio Python (análisis LLM) y servir/persistir resultados en `MongoDB`.
- MVP: recibir una URL, delegar el análisis a Python, almacenar el resultado y exponerlo vía API.

Arquitectura (breve)
- Flujo: Frontend (externo) → Backend .NET (esta API) → Microservicio Python (LLM) → MongoDB.
- Decisión MVP: el servicio Python realiza el análisis y persiste el documento `Analysis` en Mongo; .NET consume la respuesta y expone endpoints para la UI.

Requisitos
- .NET 10 SDK
- MongoDB (local o hosted)
- Servicio Python con endpoint `POST /api/analyze` que persista en Mongo y devuelva `id`
- Paquetes NuGet recomendados: `MongoDB.Driver`, `DotNetEnv`

Variables de entorno (ej. `.env`)
- `MONGO_CONNECTION_STRING` — cadena de conexión a MongoDB.
- `MONGO_DATABASE` — nombre de la base de datos.
- `PYTHON_API_URL` — URL base del servicio Python (ej.: `http://localhost:8000`).
- `PYTHON_API_KEY` — (opcional) API key para autenticar con Python.

Modelo de datos (resumen)
Documento `Analysis` (colección `analyses`) — campos principales:
- `Id` (ObjectId como string)
- `Url`
- `Category`
- `Score` (int)
- `Signals` (objeto: `promesasLaborales`, `lenguajeExagerado`, `faltaTransparencia`, `autoridadDudosa`)
- `Explanation` (string)
- `CreatedAt` (ISO 8601)
- `ModelVersion` (string)
- `Raw` (opcional, salida cruda del LLM)

Endpoints principales (API .NET)
- POST `/api/analysis`
  - Descripción: envía `url` al servicio Python; espera respuesta (Python persiste en Mongo) y devuelve la representación guardada.
  - Request (JSON):
    ```json
    {
      "url": "https://ejemplo.com/oferta/123",
      "requestId": "opcional-uuid",
      "metadata": { "language": "es" }
    }
    ```
  - Response (200 OK): objeto `Analysis` guardado con `id`.
  - Códigos: `200` (OK), `400` (input inválido), `502/503` (error con Python), `500` (error interno).

- GET `/api/analysis/{id}`
  - Descripción: obtiene resultado por `id` (ObjectId).
  - Response: objeto `Analysis` o `404` si no existe.

Contrato esperado con la API Python (resumen)
- Endpoint Python: `POST /api/analyze`
- Payload mínimo:
```json
{
  "url": "...",                 // opcional si envías "html"
  "html": null,                 // opcional
  "requestId": "...",            // opcional para correlación
  "metadata": {...}             // opcional
}
```
- Comportamiento Python:
  1. Validar input y (si procede) obtener/normalizar contenido.
  2. Ejecutar análisis LLM y calcular `signals`, `score` y `category`.
  3. Construir documento `Analysis`.
  4. Persistir en Mongo (colección `analyses`).
  5. Devolver JSON (al menos):
    ```json
    {
      "ok": true,
      "id": "",
      "url": "...",
      "category": "...",
      "score": 72,
      "signals": {
        "promesasLaborales": 30,
        "lenguajeExagerado": 20,
        ...
      },
      "explanation": "...",
      "createdAt": "2026-01-15T12:34:56Z",
      "modelVersion": "py-llm-v1",
      "raw": { /* opcional */ }
    }
    ```
  - En errores: formato uniforme:
    ```json
    {
      "ok": false,
      "error": {
        "code": "STRING",
        "message": "..."
      }
    }
    ```

Ejemplos (curl)
- Solicitar análisis:
  ```bash
  curl -X POST http://localhost:5000/api/analysis
  -H "Content-Type: application/json"
  -d '{"url":"https://ejemplo.com/oferta/123"}'
  ```
- Obtener resultado:
  ```bash
  curl http://localhost:5000/api/analysis/{id}
  ```

Ejecución local (rápido)
1. Crear/editar `.env` con `MONGO_CONNECTION_STRING`, `MONGO_DATABASE`, `PYTHON_API_URL` y (opcional) `PYTHON_API_KEY`.
2. Levantar MongoDB (local o Docker).
3. Ejecutar servicio Python (ej. FastAPI + Uvicorn) y verificar `POST /api/analyze`.
4. Ejecutar la API .NET (`dotnet run` o desde Visual Studio 2026).
5. Probar con curl/Postman los endpoints `POST /api/analysis` y `GET /api/analysis/{id}`.

Buenas prácticas y recomendaciones
- Seguridad: comunicación TLS y autenticación entre servicios (`Authorization` / `X-Api-Key` o JWT).
- Correlación: usar `X-Request-Id` en peticiones entre servicios para trazabilidad.
- Resiliencia: `HttpClient` con timeout y retry/backoff; considerar circuit-breaker.
- Idempotencia: evitar duplicados (buscar por `url` o usar hashing).
- Observabilidad: logs estructurados, métricas y guardar `modelVersion` y `raw` para auditoría.
- Versionado: exponer OpenAPI/Swagger en desarrollo y versionar endpoints (v1).

Siguientes pasos recomendados (priorizados)
1. Implementar stub minimal en Python (FastAPI + Motor/PyMongo) que persista en Mongo y devuelva `id`.
2. Integrar `PythonAnalysisService` en .NET y comprobar flujo end-to-end.
3. Añadir pruebas de integración y `docker-compose` (mongo + python + dotnet) para desarrollo.
4. Implementar autenticación/usuarios y pruebas de seguridad antesde producción.

Archivos clave en este repositorio
- `Program.cs` — configuración y DI.
- `Controllers/AnalysisController.cs` — endpoints API.
- `Services/PythonAnalysisService.cs` — cliente HTTP para Python.
- `Repositories/AnalysisRepository.cs` — persistencia en Mongo.
- `Models/Analysis.cs`, `Models/AnalysisSignals.cs` — modelos.

Notas finales
- Este README está pensado para el MVP. Antes de ampliar funcionalidades de usuarios y seguridad, publica y versiona los OpenAPI (Python y .NET) y añade pruebas de integración.
