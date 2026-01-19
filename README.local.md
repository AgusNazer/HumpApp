# HumoApp — API .NET (MVP)

Resumen
- Backend principal en `.NET 10` (C# 14).  
- Responsabilidad: exponer la API, orquestar llamadas al microservicio Python (análisis LLM), y persistir resultados en `MongoDB`.  
- MVP: recibir una URL, delegar el análisis a Python, almacenar el resultado y exponerlo vía API.

Requisitos
- .NET 10 SDK
- MongoDB (local o hosted)
- Servicio Python con endpoint `POST /api/analyze` que persista en Mongo y devuelva `id` (ver contrato)
- Paquetes NuGet recomendados: `MongoDB.Driver`, `DotNetEnv`

Variables de entorno (ej. `.env`)
- `MONGO_CONNECTION_STRING` — cadena de conexión a MongoDB.
- `MONGO_DATABASE` — nombre de la base de datos.
- `PYTHON_API_URL` — URL base del servicio Python (ej.: `http://localhost:8000`).
- `PYTHON_API_KEY` — (opcional) API key para autenticar con Python.

Endpoints principales
- POST `/api/analysis`
  - Descripción: solicita al servicio Python el análisis de una `url`. Recibe resultado y lo guarda en la colección `analyses`.
  - Request (JSON):
    ```json
    { "url": "https://ejemplo.com/oferta/123", "requestId": "opcional-uuid", "metadata": { "language": "es" } }
    ```
  - Response (200 OK): objeto `Analysis` guardado con `id` (ObjectId), `category`, `score`, `signals`, `explanation`, `createdAt`, `modelVersion`.
  - Códigos: `200`, `400` (input), `502/503` (python), `500` (interno).

- GET `/api/analysis/{id}`
  - Descripción: obtener resultado por `id` (ObjectId).
  - Response: objeto `Analysis` o `404` si no existe.

Contrato con la API Python (resumen)
- Endpoint: `POST /api/analyze`
- Payload esperado:
- Comportamiento Python (recomendado para este flujo):
- Ejecuta análisis (LLM), construye documento `Analysis`.
- Persiste en Mongo en la colección `analyses`.
- Devuelve JSON con al menos:
  ```json
  { "ok": true, "id": "<ObjectId-as-string>", "url": "...", "category": "...", "score": 72, "signals": {...}, "explanation": "...", "createdAt": "ISO8601", "modelVersion": "py-llm-v1" }
  ```
- Errores: formato uniforme `{ "ok": false, "error": { "code", "message" } }`.