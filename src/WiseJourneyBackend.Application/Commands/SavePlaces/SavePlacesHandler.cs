using MediatR;
using WiseJourneyBackend.Application.Extensions.Mappings.Places;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Exceptions;
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
        if (command.Places == null || command.Places.Count == 0)
        {
            throw new ArgumentIsNullException(nameof(command.Places));
        }

        ValidatePlaces(command.Places);

        var places = command.ToEntities();
        await _placeRepository.AddRangeAsync(places);

        return Unit.Value;
    }

    private void ValidatePlaces(List<SavePlace> places)
    {
        var invalidPlace = places.FirstOrDefault(place => string.IsNullOrWhiteSpace(place.Name));
        if (invalidPlace != null)
        {
            throw new BadRequestException("Place data is invalid.");
        }
    }
}