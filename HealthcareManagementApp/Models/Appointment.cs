namespace HealthcareManagementApp.Models;

public class Appointment
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string? Reason { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string Location { get; set; } = string.Empty;
}
