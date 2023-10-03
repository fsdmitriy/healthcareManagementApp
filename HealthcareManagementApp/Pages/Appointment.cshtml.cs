using HealthcareManagementApp.Models;
using HealthcareManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthcareManagementApp.Pages;

[IgnoreAntiforgeryToken]
public class AppointmentModel : PageModel
{
    private readonly IRepository<Appointment> _appointmentRepository;

    [BindProperty]
    public IEnumerable<Appointment> Appointments { get; set; } = Enumerable.Empty<Appointment>();
    public Appointment Appointment { get; private set; } = null!;


    public AppointmentModel(IRepository<Appointment> appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id.HasValue)
        {
            Appointment = await _appointmentRepository.GetByIdAsync(id.Value);

            if (Appointment == null)
            {
                return NotFound();
            }
        }
        else
        {
            Appointments = await _appointmentRepository.GetAllAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Appointment appointment)
    {
        if (appointment.Id > 0)
        {
            return await OnPutAsync(appointment);
        }

        await _appointmentRepository.AddAsync(appointment);

        return RedirectToPage("./Appointment", new { id = appointment.Id });
    }

    public async Task<IActionResult> OnPutAsync(Appointment appointment)
    {
        await _appointmentRepository.UpdateAsync(appointment);

        return RedirectToPage("./Appointment", new { id = appointment.Id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        await _appointmentRepository.DeleteAsync(id);

        return Page();
    }
}

