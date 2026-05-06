using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Api.DTOs;

public record TrainingClassRequest(
    string Title,
    string Description,
    ClassCategory Category,
    DifficultyLevel DifficultyLevel,
    DateTime StartTime,
    DateTime EndTime,
    Location Location,
    int Capacity,
    int AvailableSpots,
    Guid TrainerId);
