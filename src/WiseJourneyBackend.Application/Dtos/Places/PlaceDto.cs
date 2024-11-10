using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Application.Dtos.Places;
public record PlaceDto(Guid Id, string Name, string FullAddress, string ShortAddress, double Rating, int UserRatingTotal, int PriceLevel,
    GeometryDto GeometryDto, OpeningHourDto OpeningHourDto, List<PlaceTypeDto> PlaceTypeDtos);

public record GeometryDto(Guid PlaceId, double Latitude, double Longitude);

public record OpeningHourDto(Guid PlaceId, bool OpenNow, List<WeeklyHourDto> WeeklyHourDtos);

public record WeeklyHourDto(Day Day, DateTime OpenTime, DateTime CloseTime, Guid OpeningHourId);

public record PlaceTypeDto(Guid PlaceId, string TypeName);

//temporary list of records to test response from GoogleApi
// TODO map searchnearbyrequest to Dto in extension