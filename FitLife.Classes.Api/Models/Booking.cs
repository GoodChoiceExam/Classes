namespace FitLife.Classes.Api.Models;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MemberId { get; set; }
    public Guid TrainingClassId { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Booked;
}
