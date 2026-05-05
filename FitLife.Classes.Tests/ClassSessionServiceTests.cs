using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Models;

namespace FitLife.Classes.Tests;

public class ClassSessionServiceTests
{
    [Test]
    public void ClassSessionRequest_HoldsClassSessionData()
    {
        var startTime = new DateTime(2026, 5, 6, 8, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddMinutes(45);

        var request = new ClassSessionRequest(
            "Morning HIIT",
            "High intensity interval training for all levels.",
            "Sara Jensen",
            "Aarhus Center - Studio 1",
            startTime,
            endTime,
            20,
            0);

        Assert.Multiple(() =>
        {
            Assert.That(request.Title, Is.EqualTo("Morning HIIT"));
            Assert.That(request.TrainerName, Is.EqualTo("Sara Jensen"));
            Assert.That(request.Capacity, Is.EqualTo(20));
            Assert.That(request.BookedCount, Is.EqualTo(0));
        });
    }

    [Test]
    public void ClassSession_DefaultValues_AreEmptyStrings()
    {
        var classSession = new ClassSession();

        Assert.Multiple(() =>
        {
            Assert.That(classSession.Title, Is.EqualTo(string.Empty));
            Assert.That(classSession.Description, Is.EqualTo(string.Empty));
            Assert.That(classSession.TrainerName, Is.EqualTo(string.Empty));
            Assert.That(classSession.Location, Is.EqualTo(string.Empty));
        });
    }
}
