using MyRecipeBook.Infrastructure.Migrations;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.API.Extensions;

public static class MigrateDatabaseExtension
{
    public static void MigrateDatabase(this WebApplication app, IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();
        var connectionString = configuration.ConnectionString();

        var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
    }
}
