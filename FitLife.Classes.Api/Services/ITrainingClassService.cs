using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Api.Services;

public interface ITrainingClassService
{
    Task<List<TrainingClass>> GetAllAsync();
    Task<TrainingClass?> GetByIdAsync(Guid id);
    Task<TrainingClass> CreateAsync(TrainingClassRequest request);
    Task<TrainingClass?> UpdateAsync(Guid id, TrainingClassRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<Booking?> BookAsync(Guid trainingClassId, BookingRequest request);
    Task<Booking?> CancelBookingAsync(Guid trainingClassId, Guid bookingId);
    Task<Booking?> AmendBookingAsync(Guid trainingClassId, Guid bookingId);
}
