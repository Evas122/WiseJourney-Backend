using Google.Api.Gax.Grpc;
using Google.Maps.AddressValidation.V1;
using Google.Maps.Places.V1;
using Google.Type;
namespace WiseJourneyBackend.Infrastructure.Services;

public class GooglePlacesService
{
    private readonly PlacesClient _placesClient;
    private readonly AddressValidationClient _addressValidationClient;

    public GooglePlacesService(PlacesClient placesClient, AddressValidationClient addressValidationClient)
    {
        _placesClient = placesClient;
        _addressValidationClient = addressValidationClient;
    }

    public async Task<SearchNearbyResponse> GetNearbyPlaces(string address)
    {
        var addressValidationRequest = new ValidateAddressRequest
        {
            Address = new PostalAddress
            {
                AddressLines = { address }
            }
        };

        var validationResponse = await _addressValidationClient.ValidateAddressAsync(addressValidationRequest);

        var geocode = validationResponse.Result?.Geocode?.Location;

        var nearbyRequest = new SearchNearbyRequest
        {
            MaxResultCount = 20,
            LocationRestriction = new SearchNearbyRequest.Types.LocationRestriction
            {
                Circle = new Circle
                {
                    Center = new LatLng
                    {
                        Latitude = geocode.Latitude,
                        Longitude = geocode.Longitude
                    },
                    Radius = 50000
                }
            },
        };

       nearbyRequest.IncludedTypes.AddRange(["restaurant", "cafe", "bar", "art_gallery", "museum"]);

        var callSettings = CallSettings.FromHeader("X-Goog-FieldMask",
            "places.Id");

        var response = await _placesClient.SearchNearbyAsync(nearbyRequest, callSettings);

        return response;
    }
}