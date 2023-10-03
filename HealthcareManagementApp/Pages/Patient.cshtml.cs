using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthcareManagementApp.Pages;

public class PatientModel : PageModel
{
    private readonly IRepository<Patient> _patientRepository;

    [BindProperty]
    public IEnumerable<Patient> Patients { get; set; } = Enumerable.Empty<Patient>();
    public Patient Patient { get; private set; } = null!;

    public PatientModel(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id.HasValue)
        {
            Patient = await _patientRepository.GetByIdAsync(id.Value);

            if (Patient == null)
            {
                return NotFound();
            }
        }
        else
        {
            Patients = await _patientRepository.GetAllAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Patient patient)
    {
        if (patient.Id > 0)
        {
            return await OnPutAsync(patient);
        }

        await _patientRepository.AddAsync(patient);

        return RedirectToPage("./Patient", new { id = patient.Id });
    }

    public async Task<IActionResult> OnPutAsync(Patient patient)
    {
        await _patientRepository.UpdateAsync(patient);

        return RedirectToPage("./Patient", new { id = patient.Id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        await _patientRepository.DeleteAsync(id);

        return Page();
    }
}

