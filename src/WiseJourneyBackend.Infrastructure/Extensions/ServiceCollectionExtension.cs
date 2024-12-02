using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDatabaseMigratorExtension();
        services.AddDateTimeProviderExtension();
        services.AddJwtExtension(configuration);
        services.AddExceptionsHandlersExtension();
        services.AddHttpContextAccessor();
        services.AddEmailExtensions(configuration);
        services.AddAuthExtensions();
        
        services.AddRepositories();

        services.AddGoogleExtension(configuration);
        services.AddGroqAiKernelExtension(configuration);
        services.AddCacheExtension();
        services.AddScoped<IRecommendationService, RecommendationService>();

        return services;
    }
}