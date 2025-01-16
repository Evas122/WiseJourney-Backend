using Google.Api.Gax.Grpc;
using Google.Maps.AddressValidation.V1;
using Google.Maps.Places.V1;
using Google.Type;
using Microsoft.Extensions.Configuration;
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
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GooglePlacesService(PlacesClient placesClient, AddressValidationClient addressValidationClient, IDateTimeProvider dateTimeProvider, IConfiguration configuration, HttpClient httpClient)
    {
        _placesClient = placesClient;
        _addressValidationClient = addressValidationClient; 
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<List<PlaceDto>> GetNearbyPlacesAsync(GooglePlacesQuery googlePlacesQuery)
    {
        var addressValidationRequest = new ValidateAddressRequest
        {
            Address = new PostalAddress
            {
                AddressLines = { googlePlacesQuery.Location }
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
                    Radius = googlePlacesQuery.Radius
                }
            },
        };

        if (googlePlacesQuery.PlaceTypes != null && googlePlacesQuery.PlaceTypes.Any())
        {
            nearbyRequest.IncludedTypes.AddRange(googlePlacesQuery.PlaceTypes);
        }

        var callSettings = CallSettings.FromHeader("X-Goog-FieldMask",
                "places.id,places.displayName,places.types,places.formattedAddress,places.shortFormattedAddress,places.rating,places.userRatingCount,places.priceLevel,places.location,places.currentOpeningHours.openNow,places.regularOpeningHours.periods,places.photos.name");

        var response = await _placesClient.SearchNearbyAsync(nearbyRequest, callSettings);

        var placeDtos = MapToPlaceDtoList(response);

        return placeDtos;
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
            PhotoId: p.Photos.FirstOrDefault()?.Name,
            Geometry: new GeometryDto(
                PlaceId: p.Id,
                Latitude: p.Location.Latitude,
                Longitude: p.Location.Longitude
            ),
            OpeningHour: new OpeningHourDto(
                PlaceId: p.Id,
                OpenNow: p.CurrentOpeningHours?.OpenNow ?? false,
                WeeklyHours: p.RegularOpeningHours?.Periods.Select(period => new WeeklyHourDto(
                    Day: (Day)period.Open.Day,
                    OpenTime: _dateTimeProvider.UtcNow.AddHours(period.Open.Hour).AddMinutes(period.Open.Minute),
                    CloseTime: _dateTimeProvider.UtcNow.AddHours(period.Close?.Hour ?? 0).AddMinutes(period.Close?.Minute ?? 0),
                    OpeningHourId: Guid.NewGuid()
                )).ToList() ?? new List<WeeklyHourDto>()
            ),
            PlaceTypes: p.Types_.Select(type => new PlaceTypeDto(
                PlaceId: p.Id,
                TypeName: type
            )).ToList()
        )).ToList();
    }

    public Task<string> GetPhotoAsync(string photoId)
    {
        if (string.IsNullOrEmpty(photoId))
        {
            throw new ArgumentException("Photo reference is required.", nameof(photoId));
        }

        var apiKey = _configuration["GoogleApi:PlaceApiKey"];
        var baseUrl = _configuration["GoogleApi:GoogleApiUrl"];
        var photoUrlSuffix = _configuration["GoogleApi:GoogleApiPhotoSufix"];

        var photoUrl = $"{baseUrl}{photoId}{photoUrlSuffix}{apiKey}";

        return Task.FromResult(photoUrl);
    }
}