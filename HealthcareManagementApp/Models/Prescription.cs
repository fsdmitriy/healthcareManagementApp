namespace HealthcareManagementApp.Models;

public class Prescription
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
}
