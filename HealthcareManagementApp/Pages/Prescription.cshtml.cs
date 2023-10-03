using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthcareManagementApp.Pages;

[IgnoreAntiforgeryToken]
public class PrescriptionModel : PageModel
{
    private readonly IRepository<Prescription> _prescriptionRepository;

    [BindProperty]
    public IEnumerable<Prescription> Prescriptions { get; set; } = Enumerable.Empty<Prescription>();
    public Prescription Prescription { get; private set; } = null!;


    public PrescriptionModel(IRepository<Prescription> prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id.HasValue)
        {
            Prescription = await _prescriptionRepository.GetByIdAsync(id.Value);

            if (Prescription == null)
            {
                return NotFound();
            }
        }
        else
        {
            Prescriptions = await _prescriptionRepository.GetAllAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Prescription prescription)
    {
        if (prescription.Id > 0)
        {
            return await OnPutAsync(prescription);
        }

        await _prescriptionRepository.AddAsync(prescription);

        return RedirectToPage("./Prescription", new { id = prescription.Id });
    }

    public async Task<IActionResult> OnPutAsync(Prescription prescription)
    {
        await _prescriptionRepository.UpdateAsync(prescription);

        return RedirectToPage("./Prescription", new { id = prescription.Id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(id);
        if (prescription == null)
        {
            return NotFound();
        }

        await _prescriptionRepository.DeleteAsync(id);

        return RedirectToPage("Prescription");
    }
}

