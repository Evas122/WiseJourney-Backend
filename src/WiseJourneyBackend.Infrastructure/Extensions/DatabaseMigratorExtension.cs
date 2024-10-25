using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Extensions;
public static class DatabaseMigratorExtension
{
    public static void AddDatabaseMigratorExtension( this IServiceCollection services)
    {
        services.AddScoped<IDatabaseMigrator, DatabaseMigrator>();
    }
}