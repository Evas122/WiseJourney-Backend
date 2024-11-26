using WiseJourneyBackend.Application.Dtos.Recommendation;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetUserPreferences;

public record GetUserPreferencesQuery : IQuery<UserPreferencesDto>;
public class GetUserPreferencesHandler : IQueryHandler<GetUserPreferencesQuery, UserPreferencesDto>
{
    private readonly IRecommendationService _recommendationService;

    public GetUserPreferencesHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<UserPreferencesDto> Handle(GetUserPreferencesQuery query, CancellationToken cancellationToken)
    {
        var userPreferences = await _recommendationService.GenerateUserPreferencesAsync();

        return userPreferences;
    }
}