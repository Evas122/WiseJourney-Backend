using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Application.Extensions.Mappings.Places;

public static class PlaceDtoExtension
{
    public static PlaceDto ToDto(this Place place)
    {
        return new PlaceDto(
            place.Id,
            place.Name,
            place.FullAddress,
            place.ShortAddress,
            place.Rating,
            place.UserRatingsTotal,
            place.PriceLevel,
            new GeometryDto(place.Geometry.PlaceId, place.Geometry.Latitude, place.Geometry.Longitude),
            new OpeningHourDto(
                place.OpeningHour.PlaceId,
                place.OpeningHour.OpenNow,
                place.OpeningHour.WeeklyHours.Select(wh => new WeeklyHourDto(
                    wh.Day,
                    wh.OpenTime,
                    wh.CloseTime,
                    wh.Id
                )).ToList()
            ),
            place.PlaceTypes.Select(pt => new PlaceTypeDto(pt.PlaceId, pt.TypeName)).ToList()
        );
    }
}