using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.GeneratePlaces;
public class GeneratePlacesHandler : ICommandHandler<GeneratePlacesCommand, List<PlaceDto>>
{
    private readonly IRecommendationService _recommendationService;

    public GeneratePlacesHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<List<PlaceDto>> Handle(GeneratePlacesCommand command, CancellationToken cancellationToken)
    {
        var recommendedPlaces = await _recommendationService.GetRecommendedPlacesAsync(command);

        return recommendedPlaces;
    }
}