namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripDetailsDto(
    string Name,
    List<TripDayDto> TripDays);