namespace WiseJourneyBackend.Infrastructure.Interfaces;

public interface IDatabaseMigrator
{
    Task EnsureMigrationAsync();
}