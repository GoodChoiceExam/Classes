using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Api.Services;

public interface IClassSessionService
{
    Task<List<ClassSession>> GetAllAsync();
    Task<ClassSession?> GetByIdAsync(string id);
    Task<ClassSession> CreateAsync(ClassSessionRequest request);
    Task<ClassSession?> UpdateAsync(string id, ClassSessionRequest request);
    Task<bool> DeleteAsync(string id);
}
