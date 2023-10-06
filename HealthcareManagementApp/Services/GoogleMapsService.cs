using Newtonsoft.Json;
using RestSharp;

namespace HealthcareManagementApp.Services;

public class GoogleMapsService
{
    private readonly IConfiguration _config;

    public GoogleMapsService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<(string, string)> GetCoordinatesFromAddress(string address)
    {
        var apiUrl = _config.GetValue<string>("GoogleMapsGeoApi:ApiUrl") ?? throw new NullReferenceException("ApiUrl parameter cannot be null");
        var apiKey = _config.GetValue<string>("GoogleMapsGeoApi:ApiKey") ?? throw new NullReferenceException("ApiKey parameter cannot be null");

        var client = new RestClient(apiUrl);
        var request = new RestRequest("/geocode/json", Method.Get);

        request.AddParameter("address", address);
        request.AddParameter("key", apiKey);

        var response = await client.ExecuteAsync(request);
        var data = JsonConvert.DeserializeObject<dynamic>(response.Content ?? throw new NullReferenceException(nameof(response.Content)))
            ?? throw new NullReferenceException("Cannot handle null data");

        if (data.results[0] == null)
        {
            return (string.Empty, string.Empty);
        }

        var location = data.results[0]?.geometry.location;

        return (location?.lat?.ToString(), location?.lng?.ToString());
    }
}
