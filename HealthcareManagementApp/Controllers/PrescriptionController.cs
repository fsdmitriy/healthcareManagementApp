using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IRepository<Prescription> _prescriptionRepository;

    public PrescriptionController(IRepository<Prescription> prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _prescriptionRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(id);

        if (prescription == null)
        {
            return NotFound();
        }

        return Ok(prescription);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Prescription prescription)
    {
        await _prescriptionRepository.AddAsync(prescription);

        return CreatedAtAction(nameof(Create), new { id = prescription.Id }, prescription);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Prescription prescription)
    {
        if (id != prescription.Id)
        {
            return BadRequest();
        }

        await _prescriptionRepository.UpdateAsync(prescription);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _prescriptionRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        await _prescriptionRepository.DeleteAsync(id);

        return NoContent();
    }
}
