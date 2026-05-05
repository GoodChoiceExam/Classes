namespace FitLife.Classes.Api.DTOs;

public record ClassSessionRequest(
    string Title,
    string Description,
    string TrainerName,
    string Location,
    DateTime StartTime,
    DateTime EndTime,
    int Capacity,
    int BookedCount);
