namespace WiseJourneyBackend.Application.Dtos.Recommendation;

public record UserPreferencesDto(
    string DestinationType,
    decimal Budget,
    int DurationInDays
    , List<string> TravelStyles,
    string ClimatPreference,
    string AccommodationType,
    List<string> TransportTypes,
    List<string> Cuisines,
    int MaxDistanceKm,
    List<string> Activities,
    List<string> SpecificLocations,
    string DeparturePoint);