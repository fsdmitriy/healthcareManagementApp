using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IRepository<Appointment> _appointmentRepository;

    public AppointmentController(IRepository<Appointment> appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    [HttpGet]
    public Task<List<Appointment>> GetAll()
    {
        return _appointmentRepository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        return appointment;
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> Create(Appointment appointment)
    {
        await _appointmentRepository.AddAsync(appointment);

        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
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
}
