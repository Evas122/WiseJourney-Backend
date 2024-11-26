namespace WiseJourneyBackend.Application.Dtos.Recommendation;

public record UserPreferencesDto(string DestinationType, string Country, decimal Budget,
    int DurationInDays, string TravelStyle, string ClimatPreference,
    string AccommodationType, string TransportyType, float MinRating,
    string Cuisine, int MaxDistanceKm, List<string> Activities, DateTime PrefferedDate);