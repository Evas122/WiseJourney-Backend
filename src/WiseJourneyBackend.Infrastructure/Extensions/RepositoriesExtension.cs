using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Repositories;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class RepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
    }
}