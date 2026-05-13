using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Classes.Api.Controllers;

[ApiController]
[Route("api/classes")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly ITrainingClassService _trainingClassService;
    private readonly ILogger<ClassesController> _logger;

    public ClassesController(ITrainingClassService trainingClassService, ILogger<ClassesController> logger)
    {
        _trainingClassService = trainingClassService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all training classes");
        return Ok(await _trainingClassService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching training class {TrainingClassId}", id);
        var trainingClass = await _trainingClassService.GetByIdAsync(id);
        if (trainingClass is null)
        {
            _logger.LogWarning("Training class {TrainingClassId} was not found", id);
            return NotFound();
        }

        return Ok(trainingClass);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TrainingClassRequest request)
    {
        var trainingClass = await _trainingClassService.CreateAsync(request);
        _logger.LogInformation("Created training class {TrainingClassId}", trainingClass.Id);
        return CreatedAtAction(nameof(GetById), new { id = trainingClass.Id }, trainingClass);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, TrainingClassRequest request)
    {
        var trainingClass = await _trainingClassService.UpdateAsync(id, request);
        if (trainingClass is null)
        {
            _logger.LogWarning("Cannot update training class {TrainingClassId}; it was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Updated training class {TrainingClassId}", id);
        return Ok(trainingClass);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _trainingClassService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Cannot delete training class {TrainingClassId}; it was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Deleted training class {TrainingClassId}", id);
        return NoContent();
    }

    [HttpPost("{id:guid}/bookings")]
    public async Task<IActionResult> Book(Guid id, BookingRequest request)
    {
        try
        {
            var booking = await _trainingClassService.BookAsync(id, request);
            if (booking is null)
            {
                _logger.LogWarning("Cannot book training class {TrainingClassId}; it was not found", id);
                return NotFound();
            }

            _logger.LogInformation(
                "Created booking {BookingId} for member {MemberId} on training class {TrainingClassId}",
                booking.Id,
                booking.MemberId,
                id);
            return Created($"/api/classes/{id}/bookings/{booking.Id}", booking);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Booking failed for training class {TrainingClassId}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/bookings/{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, Guid bookingId)
    {
        var booking = await _trainingClassService.CancelBookingAsync(id, bookingId);
        if (booking is null)
        {
            _logger.LogWarning("Cannot cancel booking {BookingId} on training class {TrainingClassId}", bookingId, id);
            return NotFound();
        }

        _logger.LogInformation("Cancelled booking {BookingId} on training class {TrainingClassId}", bookingId, id);
        return Ok(booking);
    }

    [HttpPut("{id:guid}/bookings/{bookingId:guid}/amend")]
    public async Task<IActionResult> AmendBooking(Guid id, Guid bookingId)
    {
        var booking = await _trainingClassService.AmendBookingAsync(id, bookingId);
        if (booking is null)
        {
            _logger.LogWarning("Cannot amend booking {BookingId} on training class {TrainingClassId}", bookingId, id);
            return NotFound();
        }

        _logger.LogInformation("Amended booking {BookingId} on training class {TrainingClassId}", bookingId, id);
        return Ok(booking);
    }
    
    [HttpGet("bookings/mine")]
    public async Task<IActionResult> GetMyBookings([FromQuery] Guid memberId)
    {
        var bookings = await _trainingClassService.GetBookingsByMemberAsync(memberId);
        return Ok(bookings);
    }
}
