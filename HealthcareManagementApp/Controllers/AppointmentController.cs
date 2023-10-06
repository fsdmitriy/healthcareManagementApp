using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using HealthcareManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly GoogleMapsService _googleMapsService;

    public AppointmentController(IRepository<Appointment> appointmentRepository, GoogleMapsService googleMapsService)
    {
        _appointmentRepository = appointmentRepository;
        _googleMapsService = googleMapsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _appointmentRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        await _appointmentRepository.AddAsync(appointment);

        return CreatedAtAction(nameof(Create), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Appointment appointment)
    {
        if (id != appointment.Id)
        {
            return BadRequest();
        }

        await _appointmentRepository.UpdateAsync(appointment);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        await _appointmentRepository.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("{id}/coordinates")]
    public async Task<IActionResult> GetCoordinates(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        var address = appointment.Location;
        var coordinates = await _googleMapsService.GetCoordinatesFromAddress(address);

        return Ok(new { Latitude = coordinates.Item1, Longitude = coordinates.Item2 });
    }
}
