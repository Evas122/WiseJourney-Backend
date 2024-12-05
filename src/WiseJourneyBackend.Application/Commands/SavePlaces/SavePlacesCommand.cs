using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Application.Commands.SavePlaces;

public record SavePlacesCommand(
    List<SavePlace> Places) : ICommand<Unit>;
    
public record SavePlace(
    string Id,
    string Name,
    string FullAddress,
    string ShortAddress,
    double Rating,
    int UserRatingTotal,
    int PriceLevel,
    SaveGeometry Geometry,
    SaveOpeningHour OpeningHour,
    List<SavePlaceType> PlaceTypes);

public record SaveGeometry(
    double Latitude,
    double Longitude);

public record SaveOpeningHour(
    bool OpenNow,
    List<SaveWeeklyHour> WeeklyHours);

public record SaveWeeklyHour(
    Day Day,
    DateTime OpenTime,
    DateTime CloseTime);

public record SavePlaceType(
    string TypeName);