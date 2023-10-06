namespace HealthcareManagementApp.Models;

public class GoogleMapsGeocodingResult
{
    public string FormattedAddress { get; set; }

    public GoogleMapsGeocodingGeometry Geometry { get; set; }
}

public class GoogleMapsGeocodingGeometry
{
    public GoogleMapsGeocodingLocation Location { get; set; }
}


public class GoogleMapsGeocodingLocation
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }
}