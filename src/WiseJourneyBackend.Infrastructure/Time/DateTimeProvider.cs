using WiseJourneyBackend.Application.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Time;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}