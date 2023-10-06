namespace HealthcareManagementApp.Models;

public class GoogleMapsGeocodingResponse
{
    public string Status { get; set; }

    public GoogleMapsGeocodingResult[] Results { get; set; }
}
