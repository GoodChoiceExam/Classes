using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;
using MongoDB.Driver;

namespace FitLife.Classes.Api.Services;

public class TrainingClassService : ITrainingClassService
{
    private readonly IMongoCollection<TrainingClass> _classes;

    public TrainingClassService(IConfiguration configuration)
    {
        var mongoConn = configuration["MongoDB:ConnectionString"]!;
        var mongoDb = configuration["MongoDB:DatabaseName"]!;
        var collectionName = configuration["MongoDB:CollectionName"] ?? "trainingClasses";

        var client = new MongoClient(mongoConn);
        var database = client.GetDatabase(mongoDb);
        _classes = database.GetCollection<TrainingClass>(collectionName);
    }

    public async Task<List<TrainingClass>> GetAllAsync()
    {
        return await _classes.Find(_ => true).ToListAsync();
    }

    public async Task<TrainingClass?> GetByIdAsync(Guid id)
    {
        return await _classes.Find(trainingClass => trainingClass.Id == id).FirstOrDefaultAsync();
    }

    public async Task<TrainingClass> CreateAsync(TrainingClassRequest request)
    {
        var trainingClass = ToTrainingClass(request);
        await _classes.InsertOneAsync(trainingClass);
        return trainingClass;
    }

    public async Task<TrainingClass?> UpdateAsync(Guid id, TrainingClassRequest request)
    {
        var existing = await GetByIdAsync(id);
        if (existing is null)
            return null;

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Category = request.Category;
        existing.DifficultyLevel = request.DifficultyLevel;
        existing.StartTime = request.StartTime;
        existing.EndTime = request.EndTime;
        existing.Location = request.Location;
        existing.Capacity = request.Capacity;
        existing.AvailableSpots = request.AvailableSpots;
        existing.TrainerId = request.TrainerId;

        await _classes.ReplaceOneAsync(trainingClass => trainingClass.Id == id, existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _classes.DeleteOneAsync(trainingClass => trainingClass.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Booking?> BookAsync(Guid trainingClassId, BookingRequest request)
    {
        var trainingClass = await GetByIdAsync(trainingClassId);
        if (trainingClass is null)
            return null;

        var booking = trainingClass.Book(request.MemberId);
        await _classes.ReplaceOneAsync(existing => existing.Id == trainingClassId, trainingClass);
        return booking;
    }

    public async Task<Booking?> CancelBookingAsync(Guid trainingClassId, Guid bookingId)
    {
        var trainingClass = await GetByIdAsync(trainingClassId);
        if (trainingClass is null)
            return null;

        var booking = trainingClass.CancelBooking(bookingId);
        if (booking is null)
            return null;

        await _classes.ReplaceOneAsync(existing => existing.Id == trainingClassId, trainingClass);
        return booking;
    }

    public async Task<Booking?> AmendBookingAsync(Guid trainingClassId, Guid bookingId)
    {
        var trainingClass = await GetByIdAsync(trainingClassId);
        if (trainingClass is null)
            return null;

        var booking = trainingClass.AmendBooking(bookingId);
        if (booking is null)
            return null;

        await _classes.ReplaceOneAsync(existing => existing.Id == trainingClassId, trainingClass);
        return booking;
    }

    private static TrainingClass ToTrainingClass(TrainingClassRequest request) => new()
    {
        Title = request.Title,
        Description = request.Description,
        Category = request.Category,
        DifficultyLevel = request.DifficultyLevel,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        Location = request.Location,
        Capacity = request.Capacity,
        AvailableSpots = request.AvailableSpots,
        TrainerId = request.TrainerId
    };
}
