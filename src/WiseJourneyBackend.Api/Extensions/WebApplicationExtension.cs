using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Api.Extensions;
public static class WebApplicationExtension
{
    public static async Task EnsureDatabaseMigratedAsync(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
        await migrator.EnsureMigrationAsync();
    }
}