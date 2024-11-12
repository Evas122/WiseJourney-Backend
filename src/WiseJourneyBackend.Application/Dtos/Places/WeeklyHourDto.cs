using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Application.Dtos.Places;

public record WeeklyHourDto(Day Day, DateTime OpenTime, DateTime CloseTime, Guid OpeningHourId);