using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetPhoto;

public class GetPhotoQueryHandler : IQueryHandler<GetPhotoQuery, string>
{
    private readonly IGooglePlacesService _googlePlacesService;

    public GetPhotoQueryHandler(IGooglePlacesService googlePlacesService)
    {
        _googlePlacesService = googlePlacesService;
    }

    public async Task<string> Handle(GetPhotoQuery request, CancellationToken cancellationToken)
    {
        var photoUrl = await _googlePlacesService.GetPhotoAsync(request.PhotoId);

        return photoUrl;
    }
}