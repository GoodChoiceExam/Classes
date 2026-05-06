namespace FitLife.Classes.Api.Models;

public class TrainingClass
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ClassCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Location Location { get; set; }
    public int Capacity { get; set; }
    public int AvailableSpots { get; set; }
    public Guid TrainerId { get; set; }
    public List<Booking> Bookings { get; set; } = [];

    public Booking Book(Guid memberId)
    {
        if (AvailableSpots <= 0)
            throw new InvalidOperationException("No available spots for this class.");

        if (Bookings.Any(booking => booking.MemberId == memberId && booking.Status == BookingStatus.Booked))
            throw new InvalidOperationException("Member already has an active booking for this class.");

        var booking = new Booking
        {
            MemberId = memberId,
            TrainingClassId = Id,
            BookedAt = DateTime.UtcNow,
            Status = BookingStatus.Booked
        };

        Bookings.Add(booking);
        AvailableSpots--;
        return booking;
    }

    public Booking? CancelBooking(Guid bookingId)
    {
        var booking = Bookings.FirstOrDefault(existing => existing.Id == bookingId);
        if (booking is null)
            return null;

        if (booking.Status == BookingStatus.Booked)
            AvailableSpots++;

        booking.Status = BookingStatus.Cancelled;
        return booking;
    }

    public Booking? AmendBooking(Guid bookingId)
    {
        var booking = Bookings.FirstOrDefault(existing => existing.Id == bookingId);
        if (booking is null)
            return null;

        booking.Status = BookingStatus.Amended;
        return booking;
    }
}
