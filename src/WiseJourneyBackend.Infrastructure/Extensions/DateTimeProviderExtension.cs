using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Time;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class DateTimeProviderExtension
{
    public static void AddDateTimeProviderExtension(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}