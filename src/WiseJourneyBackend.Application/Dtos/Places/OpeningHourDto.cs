namespace WiseJourneyBackend.Application.Dtos.Places;

public record OpeningHourDto(
    string PlaceId,
    bool OpenNow,
    List<WeeklyHourDto> WeeklyHours);