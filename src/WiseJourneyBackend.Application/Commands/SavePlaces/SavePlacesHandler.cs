using MediatR;
using WiseJourneyBackend.Application.Extensions.Mappings.Places;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Commands.SavePlaces;
public class SavePlacesHandler : ICommandHandler<SavePlacesCommand, Unit>
{
    private readonly IPlaceRepository _placeRepository;

    public SavePlacesHandler(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
    }

    public async Task<Unit> Handle(SavePlacesCommand command, CancellationToken cancellationToken)
    {
        var places = command.ToEntities();
        await _placeRepository.AddRangeAsync(places);

        return Unit.Value;
    }
}