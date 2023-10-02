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
    public IEnumerable<Patient> GetAll()
    {
        return _patientRepository.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        await _patientRepository.AddAsync(patient);

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
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
