using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class CacheExtension
{
    public static void AddCacheExtension(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
    }
}