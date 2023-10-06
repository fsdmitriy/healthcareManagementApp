using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IRepository<Patient> _patientRepository;

    public PatientsController(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _patientRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Patient patient)
    {
        await _patientRepository.AddAsync(patient);

        return CreatedAtAction(nameof(Create), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Patient patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        await _patientRepository.UpdateAsync(patient);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        await _patientRepository.DeleteAsync(id);

        return NoContent();
    }
}
