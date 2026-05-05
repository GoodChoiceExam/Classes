using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;
using MongoDB.Driver;

namespace FitLife.Classes.Api.Services;

public class ClassSessionService : IClassSessionService
{
    private readonly IMongoCollection<ClassSession> _classes;

    public ClassSessionService(IConfiguration configuration)
    {
        var mongoConn = configuration["MongoDB:ConnectionString"]!;
        var mongoDb = configuration["MongoDB:DatabaseName"]!;
        var collectionName = configuration["MongoDB:CollectionName"] ?? "classSessions";

        var client = new MongoClient(mongoConn);
        var database = client.GetDatabase(mongoDb);
        _classes = database.GetCollection<ClassSession>(collectionName);
    }

    public async Task<List<ClassSession>> GetAllAsync()
    {
        return await _classes.Find(_ => true).ToListAsync();
    }

    public async Task<ClassSession?> GetByIdAsync(string id)
    {
        return await _classes.Find(classSession => classSession.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ClassSession> CreateAsync(ClassSessionRequest request)
    {
        var classSession = ToClassSession(request);
        await _classes.InsertOneAsync(classSession);
        return classSession;
    }

    public async Task<ClassSession?> UpdateAsync(string id, ClassSessionRequest request)
    {
        var classSession = ToClassSession(request);
        classSession.Id = id;

        var result = await _classes.ReplaceOneAsync(existing => existing.Id == id, classSession);
        return result.MatchedCount == 0 ? null : classSession;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _classes.DeleteOneAsync(classSession => classSession.Id == id);
        return result.DeletedCount > 0;
    }

    private static ClassSession ToClassSession(ClassSessionRequest request) => new()
    {
        Title = request.Title,
        Description = request.Description,
        TrainerName = request.TrainerName,
        Location = request.Location,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        Capacity = request.Capacity,
        BookedCount = request.BookedCount
    };
}
