namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripDayDto(
    int Day,
    List<TripPlaceDto> TripPlaces);