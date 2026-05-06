using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Tests;

public class TrainingClassTests
{
    [Test]
    public void Book_WhenSpotIsAvailable_AddsBookingAndReducesAvailableSpots()
    {
        var trainingClass = CreateTrainingClass(availableSpots: 2);
        var memberId = Guid.NewGuid();

        var booking = trainingClass.Book(memberId);

        Assert.Multiple(() =>
        {
            Assert.That(booking.MemberId, Is.EqualTo(memberId));
            Assert.That(booking.TrainingClassId, Is.EqualTo(trainingClass.Id));
            Assert.That(booking.Status, Is.EqualTo(BookingStatus.Booked));
            Assert.That(trainingClass.AvailableSpots, Is.EqualTo(1));
            Assert.That(trainingClass.Bookings, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void Book_WhenNoSpotsAreAvailable_ThrowsException()
    {
        var trainingClass = CreateTrainingClass(availableSpots: 0);

        Assert.Throws<InvalidOperationException>(() => trainingClass.Book(Guid.NewGuid()));
    }

    [Test]
    public void CancelBooking_WhenBookingWasActive_CancelsAndRestoresAvailableSpot()
    {
        var trainingClass = CreateTrainingClass(availableSpots: 1);
        var booking = trainingClass.Book(Guid.NewGuid());

        var cancelled = trainingClass.CancelBooking(booking.Id);

        Assert.Multiple(() =>
        {
            Assert.That(cancelled, Is.Not.Null);
            Assert.That(cancelled!.Status, Is.EqualTo(BookingStatus.Cancelled));
            Assert.That(trainingClass.AvailableSpots, Is.EqualTo(1));
        });
    }

    [Test]
    public void AmendBooking_WhenBookingExists_UpdatesStatus()
    {
        var trainingClass = CreateTrainingClass(availableSpots: 1);
        var booking = trainingClass.Book(Guid.NewGuid());

        var amended = trainingClass.AmendBooking(booking.Id);

        Assert.Multiple(() =>
        {
            Assert.That(amended, Is.Not.Null);
            Assert.That(amended!.Status, Is.EqualTo(BookingStatus.Amended));
        });
    }

    private static TrainingClass CreateTrainingClass(int availableSpots) => new()
    {
        Title = "Morning HIIT",
        Description = "High intensity interval training for all levels.",
        Category = ClassCategory.HIIT,
        DifficultyLevel = DifficultyLevel.Beginner,
        StartTime = new DateTime(2026, 5, 6, 8, 0, 0, DateTimeKind.Utc),
        EndTime = new DateTime(2026, 5, 6, 8, 45, 0, DateTimeKind.Utc),
        Location = Location.Vesterbro,
        Capacity = 20,
        AvailableSpots = availableSpots,
        TrainerId = Guid.NewGuid()
    };
}
