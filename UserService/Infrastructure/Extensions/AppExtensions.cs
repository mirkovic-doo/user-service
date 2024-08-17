using Cassandra.Data.Linq;
using Cassandra.Mapping;
using UserService.Contracts.Data;
using UserService.Contracts.Providers;
using UserService.Infrastructure.Helpers;

namespace UserService.Infrastructure.Extensions;

public static class AppExtensions
{
    public static void Migrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var databaseSession = scope.ServiceProvider.GetRequiredService<IDatabaseSessionProvider>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var session = databaseSession.Session;

        session.Execute("CREATE TABLE IF NOT EXISTS migrations (id UUID PRIMARY KEY, name text);");

        var migrationsTable = new Table<Migration>(session, MappingConfiguration.Global, "migrations");

        var appliedMigrations = migrationsTable.Execute().Select(m => m.Name).ToList();

        appliedMigrations = appliedMigrations.ConvertAll(d => d.ToLower());

        var localMigrations = FileHelper.GetFileNamesFromFolder("Migrations");

        foreach (var localMigration in localMigrations)
        {
            if (!appliedMigrations.Contains(localMigration.ToLower()))
            {
                logger.LogInformation($"Applying migration {localMigration}...");
                ApplyMigration(localMigration, session, logger);

                session.Execute($@"INSERT INTO migrations (id, name) VALUES ({Guid.NewGuid()}, '{localMigration}')");
            }
        }
    }

    private static void ApplyMigration(string migrationName, Cassandra.ISession session, ILogger logger)
    {
        try
        {
            string migrationData = File.ReadAllText($"Migrations/{migrationName}.cql");
            string[] migrationStatements = migrationData.Split(";", StringSplitOptions.RemoveEmptyEntries);

            foreach (var statement in migrationStatements)
            {
                var trimmedStatement = statement.Trim();

                if (!string.IsNullOrEmpty(trimmedStatement))
                {
                    session.Execute(trimmedStatement);
                }
            }

            logger.LogInformation($"Applied migration {migrationName}.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Applying migration {migrationName} failed, reason: {ex.Message}.");
            throw;
        }
    }
}
