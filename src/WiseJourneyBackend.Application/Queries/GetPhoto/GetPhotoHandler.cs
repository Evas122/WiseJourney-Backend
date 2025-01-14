using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetPhoto;

public class GetPhotoQueryHandler : IQueryHandler<GetPhotoQuery, byte[]>
{
    private readonly IGooglePlacesService _googlePlacesService;

    public GetPhotoQueryHandler(IGooglePlacesService googlePlacesService)
    {
        _googlePlacesService = googlePlacesService;
    }

    public async Task<byte[]> Handle(GetPhotoQuery request, CancellationToken cancellationToken)
    {
        var photoBytes = await _googlePlacesService.GetPhotoAsync(request.PhotoId);

        return photoBytes;
    }
}