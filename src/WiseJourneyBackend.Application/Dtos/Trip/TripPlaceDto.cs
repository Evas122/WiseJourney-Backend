using WiseJourneyBackend.Application.Dtos.Places;

namespace WiseJourneyBackend.Application.Dtos.Trip;

public record TripPlaceDto(
    string PlaceId,
    PlaceDto Place);