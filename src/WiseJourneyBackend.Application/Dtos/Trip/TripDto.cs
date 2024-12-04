namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripDto(
    Guid Id,
    string Name,
    DateTime StartDateUtc,
    DateTime EndDateUtc);