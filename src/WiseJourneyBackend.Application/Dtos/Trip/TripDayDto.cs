namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripDayDto(
    DateTime DateUtc,
    List<TripPlaceDto> TripPlaces);