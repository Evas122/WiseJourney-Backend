using WiseJourneyBackend.Application.Commands.SavePlaces;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Application.Extensions.Mappings.Places;

public static class SavePlacesCommandExtension
{
    public static List<Place> ToEntities(this SavePlacesCommand command)
    {
        var places = new List<Place>();

        foreach(var savePlace in command.Places)
        {
            var place = new Place
            {
                Id = savePlace.Id,
                Name = savePlace.Name,
                FullAddress = savePlace.FullAddress,
                ShortAddress = savePlace.ShortAddress,
                Rating = savePlace.Rating,
                UserRatingsTotal = savePlace.UserRatingTotal,
                PriceLevel = savePlace.PriceLevel,
                PhotoId = savePlace.PhotoId,
                Geometry = new Geometry
                {
                    PlaceId = savePlace.Id,
                    Latitude = savePlace.Geometry.Latitude,
                    Longitude = savePlace.Geometry.Longitude,
                },
                OpeningHour = new OpeningHour
                {
                    PlaceId = savePlace.Id,
                    OpenNow = savePlace.OpeningHour.OpenNow,
                    WeeklyHours = savePlace.OpeningHour.WeeklyHours
                    .Select(wh => new WeeklyHour
                    {
                        Id = Guid.NewGuid(),
                        Day = wh.Day,
                        OpenTime = wh.OpenTime,
                        CloseTime = wh.CloseTime,
                    }).ToList()
                },
                PlaceTypes = savePlace.PlaceTypes.Select(pt => new PlaceType
                {
                    Id = Guid.NewGuid(),
                    PlaceId = savePlace.Id,
                    TypeName = pt.TypeName
                }).ToList()
            };
            places.Add(place);
        }
        return places;
    }
}