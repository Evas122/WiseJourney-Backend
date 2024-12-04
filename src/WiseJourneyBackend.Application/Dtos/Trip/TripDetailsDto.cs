namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripDetailsDto(
    string Name,
    DateTime StartDateUtc,
    DateTime EndDateUtc,
    List<TripDayDto> TripDays);