using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetRecommendedPlaces;

public record GetRecommendedPlacesQuery : IQuery<List<PlaceDto>>;