# FitLife Classes

Classes is a small ASP.NET Core Web API microservice for fitness class sessions.

The service handles CRUD operations for fitness classes and stores data in MongoDB.

## What It Does

A class session contains:

- id
- title
- description
- trainer name
- location
- start time
- end time
- capacity
- booked count

## Run Locally

Start MongoDB first. For local development, the default configuration expects MongoDB here:

```text
mongodb://localhost:27017
```

From the repository root:

```powershell
dotnet restore FitLife.Classes.slnx
dotnet run --project FitLife.Classes.Api/FitLife.Classes.Api.csproj
```

The local launch profile uses:

```text
http://localhost:5245
```

Swagger is available here:

```text
http://localhost:5245/swagger
```

Health check:

```text
GET http://localhost:5245/healthz
```

## Endpoints

```text
GET    /api/classes
GET    /api/classes/{id}
POST   /api/classes
PUT    /api/classes/{id}
DELETE /api/classes/{id}
```

## Example Request

```http
POST /api/classes
Content-Type: application/json

{
  "title": "Morning HIIT",
  "description": "High intensity interval training for all levels.",
  "trainerName": "Sara Jensen",
  "location": "Aarhus Center - Studio 1",
  "startTime": "2026-05-06T08:00:00Z",
  "endTime": "2026-05-06T08:45:00Z",
  "capacity": 20,
  "bookedCount": 0
}
```

## Docker

Build the image:

```powershell
docker build -f FitLife.Classes.Api/Dockerfile -t fitlife-classes .
```

Run it:

```powershell
docker run -p 8080:8080 fitlife-classes
```

## Later Docker Compose And Nginx Integration

This service can later be added to the Infrastructure `docker-compose.yml` as a `classes` service.

When running in Docker Compose, the MongoDB connection string should use the MongoDB container name instead of localhost, for example:

```text
MongoDB__ConnectionString=mongodb://mongodb:27017
```

Nginx can later proxy `/api/classes` to the Classes container.

No RabbitMQ, authentication, or frontend integration is included yet.

