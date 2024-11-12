using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Infrastructure.Data;

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

        return services;
    }
}