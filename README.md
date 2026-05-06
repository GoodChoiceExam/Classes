# FitLife Classes

Classes is an ASP.NET Core Web API microservice for fitness classes and class bookings.

The service matches the Classes bounded context from the FitLife DDD model. `TrainingClass` is the aggregate root, and `Booking` is an entity inside that aggregate because bookings affect class capacity and available spots.

## What It Does

A training class contains:

- id
- title
- description
- category
- difficulty level
- start time
- end time
- location
- capacity
- available spots
- trainer id
- bookings

A booking contains:

- id
- member id
- training class id
- booked at
- status

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
POST   /api/classes/{id}/bookings
PUT    /api/classes/{id}/bookings/{bookingId}/cancel
PUT    /api/classes/{id}/bookings/{bookingId}/amend
```

## Example Create Training Class

```http
POST /api/classes
Content-Type: application/json

{
  "title": "Morning HIIT",
  "description": "High intensity interval training for all levels.",
  "category": "HIIT",
  "difficultyLevel": "Beginner",
  "startTime": "2026-05-06T08:00:00Z",
  "endTime": "2026-05-06T08:45:00Z",
  "location": "Vesterbro",
  "capacity": 20,
  "availableSpots": 20,
  "trainerId": "11111111-1111-1111-1111-111111111111"
}
```

## Example Book Class

```http
POST /api/classes/{id}/bookings
Content-Type: application/json

{
  "memberId": "22222222-2222-2222-2222-222222222222"
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
