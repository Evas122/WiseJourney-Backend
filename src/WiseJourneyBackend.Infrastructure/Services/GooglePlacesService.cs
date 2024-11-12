using Google.Api.Gax.Grpc;
using Google.Maps.AddressValidation.V1;
using Google.Maps.Places.V1;
using Google.Type;
using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Enums;
using WiseJourneyBackend.Domain.Exceptions;
namespace WiseJourneyBackend.Infrastructure.Services;

public class GooglePlacesService : IGooglePlacesService
{
    private readonly PlacesClient _placesClient;
    private readonly AddressValidationClient _addressValidationClient;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GooglePlacesService(PlacesClient placesClient, AddressValidationClient addressValidationClient, IDateTimeProvider dateTimeProvider)
    {
        _placesClient = placesClient;
        _addressValidationClient = addressValidationClient;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<List<PlaceDto>> GetNearbyPlaces(string address)
    {
        var addressValidationRequest = new ValidateAddressRequest
        {
            Address = new PostalAddress
            {
                AddressLines = { address }
            }
        };

        var validationResponse = await _addressValidationClient.ValidateAddressAsync(addressValidationRequest);

        var geocode = (validationResponse.Result?.Geocode?.Location) ?? throw new BadRequestException("invalid geocode location");
        var nearbyRequest = new SearchNearbyRequest
        {
            MaxResultCount = 10,
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
        //TODO in future add more type to create trip plan

        var callSettings = CallSettings.FromHeader("X-Goog-FieldMask",
                "places.id,places.displayName,places.types,places.formattedAddress,places.shortFormattedAddress,places.rating,places.userRatingCount,places.priceLevel,places.location,places.currentOpeningHours.openNow,places.regularOpeningHours.periods");

        var response = await _placesClient.SearchNearbyAsync(nearbyRequest, callSettings);

        return MapToPlaceDtoList(response);
    }

    private List<PlaceDto> MapToPlaceDtoList(SearchNearbyResponse response)
    {
        return response.Places.Select(p => new PlaceDto(
            Id: p.Id,
            Name: p.DisplayName.Text,
            FullAddress: p.FormattedAddress,
            ShortAddress: p.ShortFormattedAddress,
            Rating: p.Rating,
            UserRatingTotal: p.UserRatingCount,
            PriceLevel: Convert.ToInt32(p.PriceLevel),
            GeometryDto: new GeometryDto(
                PlaceId: p.Id,
                Latitude: p.Location.Latitude,
                Longitude: p.Location.Longitude
            ),
            OpeningHourDto: new OpeningHourDto(
                PlaceId: p.Id,
                OpenNow: p.CurrentOpeningHours?.OpenNow ?? false,
                WeeklyHourDtos: p.RegularOpeningHours?.Periods.Select(period => new WeeklyHourDto(
                    Day: (Day)period.Open.Day,
                    OpenTime: _dateTimeProvider.UtcNow.AddHours(period.Open.Hour).AddMinutes(period.Open.Minute),
                    CloseTime: _dateTimeProvider.UtcNow.AddHours(period.Close?.Hour ?? 0).AddMinutes(period.Close?.Minute ?? 0),
                    OpeningHourId: Guid.NewGuid()
                )).ToList() ?? new List<WeeklyHourDto>()
            ),
            PlaceTypeDtos: p.Types_.Select(type => new PlaceTypeDto(
                PlaceId: p.Id,
                TypeName: type
            )).ToList()
        )).ToList();
    }
}