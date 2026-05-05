using FitLife.Classes.Api.DTOs;
using FitLife.Classes.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLife.Classes.Api.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly IClassSessionService _classSessionService;

    public ClassesController(IClassSessionService classSessionService)
    {
        _classSessionService = classSessionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _classSessionService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var classSession = await _classSessionService.GetByIdAsync(id);
        if (classSession is null)
            return NotFound();

        return Ok(classSession);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClassSessionRequest request)
    {
        var classSession = await _classSessionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = classSession.Id }, classSession);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ClassSessionRequest request)
    {
        var classSession = await _classSessionService.UpdateAsync(id, request);
        if (classSession is null)
            return NotFound();

        return Ok(classSession);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _classSessionService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
