using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Classes.Api.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly ITrainingClassService _trainingClassService;

    public ClassesController(ITrainingClassService trainingClassService)
    {
        _trainingClassService = trainingClassService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _trainingClassService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var trainingClass = await _trainingClassService.GetByIdAsync(id);
        if (trainingClass is null)
            return NotFound();

        return Ok(trainingClass);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TrainingClassRequest request)
    {
        var trainingClass = await _trainingClassService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = trainingClass.Id }, trainingClass);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, TrainingClassRequest request)
    {
        var trainingClass = await _trainingClassService.UpdateAsync(id, request);
        if (trainingClass is null)
            return NotFound();

        return Ok(trainingClass);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _trainingClassService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id:guid}/bookings")]
    public async Task<IActionResult> Book(Guid id, BookingRequest request)
    {
        try
        {
            var booking = await _trainingClassService.BookAsync(id, request);
            if (booking is null)
                return NotFound();

            return Created($"/api/classes/{id}/bookings/{booking.Id}", booking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/bookings/{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, Guid bookingId)
    {
        var booking = await _trainingClassService.CancelBookingAsync(id, bookingId);
        if (booking is null)
            return NotFound();

        return Ok(booking);
    }

    [HttpPut("{id:guid}/bookings/{bookingId:guid}/amend")]
    public async Task<IActionResult> AmendBooking(Guid id, Guid bookingId)
    {
        var booking = await _trainingClassService.AmendBookingAsync(id, bookingId);
        if (booking is null)
            return NotFound();

        return Ok(booking);
    }
}
