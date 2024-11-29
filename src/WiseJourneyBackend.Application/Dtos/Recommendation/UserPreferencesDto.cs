namespace WiseJourneyBackend.Application.Dtos.Recommendation;

public record UserPreferencesDto(string DestinationType, decimal Budget,
    int DurationInDays, string TravelStyle, string ClimatPreference,
    string AccommodationType, string TransportyType, int NumberOfPersons,
    string Cuisine, int MaxDistanceKm, List<string> Activities,
    List<string> SpecificLocations, string UserLocation);