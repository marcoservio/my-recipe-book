using Dapper;

using Microsoft.Data.SqlClient;

using MyRecipeBook.Domain.Enums;

using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Migrations;
public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString)
    {
        if (databaseType == DatabaseType.MySQL)
            EnsureDatabaseCreatedMySQL(connectionString);
        else
            EnsureDatabaseCreatedSQLServer(connectionString);
    }

    private static void EnsureDatabaseCreatedMySQL(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");

        using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = connection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if (!records.Any())
            connection.Execute($"CREATE DATABASE {databaseName}");
    }

    private static void EnsureDatabaseCreatedSQLServer(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = connection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

        if (!records.Any())
            connection.Execute($"CREATE DATABASE {databaseName}");
    }
}
