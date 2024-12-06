using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetRecommendedPlaces;

public class GetRecommendedPlacesHandler : IQueryHandler<GetRecommendedPlacesQuery, List<PlaceDto>>
{
    private readonly IRecommendationService _recommendationService;

    public GetRecommendedPlacesHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<List<PlaceDto>> Handle(GetRecommendedPlacesQuery query, CancellationToken cancellationToken)
    {
        var recommendedPlaces = await _recommendationService.GetRecommendedPlacesAsync();

        return recommendedPlaces;
    }
}