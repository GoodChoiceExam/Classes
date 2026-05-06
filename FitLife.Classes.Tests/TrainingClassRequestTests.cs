using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Tests;

public class TrainingClassRequestTests
{
    [Test]
    public void TrainingClassRequest_HoldsDddTrainingClassData()
    {
        var trainerId = Guid.NewGuid();
        var startTime = new DateTime(2026, 5, 6, 8, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddMinutes(45);

        var request = new TrainingClassRequest(
            "Morning HIIT",
            "High intensity interval training for all levels.",
            ClassCategory.HIIT,
            DifficultyLevel.Beginner,
            startTime,
            endTime,
            Location.Vesterbro,
            20,
            20,
            trainerId);

        Assert.Multiple(() =>
        {
            Assert.That(request.Title, Is.EqualTo("Morning HIIT"));
            Assert.That(request.Category, Is.EqualTo(ClassCategory.HIIT));
            Assert.That(request.DifficultyLevel, Is.EqualTo(DifficultyLevel.Beginner));
            Assert.That(request.Location, Is.EqualTo(Location.Vesterbro));
            Assert.That(request.AvailableSpots, Is.EqualTo(20));
            Assert.That(request.TrainerId, Is.EqualTo(trainerId));
        });
    }
}
