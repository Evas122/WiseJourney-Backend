using Google.Api.Gax.Grpc;
using Google.Maps.AddressValidation.V1;
using Google.Maps.Places.V1;
using Google.Type;
using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Dtos.Recommendation;
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

    public async Task<List<PlaceDto>> GetNearbyPlacesAsync(GooglePlacesPreferencesDto googlePlacesPreferencesDto)
    {
        var addressValidationRequest = new ValidateAddressRequest
        {
            Address = new PostalAddress
            {
                AddressLines = { googlePlacesPreferencesDto.Location }
            }
        };

        var validationResponse = await _addressValidationClient.ValidateAddressAsync(addressValidationRequest);

        var geocode = (validationResponse.Result?.Geocode?.Location) ?? throw new BadRequestException("Invalid geocode location");

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
                    Radius = googlePlacesPreferencesDto.Radius
                }
            },
        };

        if (googlePlacesPreferencesDto.PlaceTypes != null && googlePlacesPreferencesDto.PlaceTypes.Any())
        {
            nearbyRequest.IncludedTypes.AddRange(googlePlacesPreferencesDto.PlaceTypes);
        }

        var callSettings = CallSettings.FromHeader("X-Goog-FieldMask",
                "places.id,places.displayName,places.types,places.formattedAddress,places.shortFormattedAddress,places.rating,places.userRatingCount,places.priceLevel,places.location,places.currentOpeningHours.openNow,places.regularOpeningHours.periods");

        var response = await _placesClient.SearchNearbyAsync(nearbyRequest, callSettings);

        var placeDtos = MapToPlaceDtoList(response);

        return placeDtos;
    }

    private List<PlaceDto> FilterResultsByPreferences(List<PlaceDto> placeDtos, GooglePlacesPreferencesDto preferences)
    {
        //TODO filter is not working property because many places have 0 price level so better filter in hotel or something else 
        var filteredResults = placeDtos;

        if (preferences.PriceLevel >= 0)
        {
            filteredResults = filteredResults.Where(p => p.PriceLevel == preferences.PriceLevel).ToList();
        }

        if (!string.IsNullOrEmpty(preferences.Keyword))
        {
            filteredResults = filteredResults.Where(p => p.Name.Contains(preferences.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                         p.FullAddress.Contains(preferences.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (preferences.Queries != null && preferences.Queries.Any())
        {
            filteredResults = filteredResults.Where(p => preferences.Queries.Any(query => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                                                                            p.FullAddress.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        return filteredResults;
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