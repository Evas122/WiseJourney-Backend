using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetPhoto;

public class GetPhotoQuery : IQuery<byte[]>
{
    public string PhotoId { get; set; }

    public GetPhotoQuery(string photoId)
    {
        PhotoId = photoId ?? throw new ArgumentNullException(nameof(photoId));
    }
}