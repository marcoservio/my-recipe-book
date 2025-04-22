using Microsoft.Extensions.Configuration;

using MyRecipeBook.Enums;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnviroment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType");

        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == Enums.DatabaseType.MySQL)
            return configuration.GetConnectionString("ConnectionMySQLServer")!;
        else
            return configuration.GetConnectionString("ConnectionSQLServer")!;
    }

    public static uint ExpirationTimeInMinutes(this IConfiguration configuration)
    {
        return configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
    }

    public static string SigningKey(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Jwt:SigningKey")!;
    }

    public static string EncripterAdditionalKey(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Password:AdditionalKey")!;
    }

    public static string OpenAIApiKey(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:OpenAI:ApiKey")!;
    }

    public static string OpenAIModel(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:OpenAI:Model")!;
    }

    public static string AzureStorageConnectionString(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:BlobStorage:Azure")!;
    }

    public static string AzureServiceBusConnectionString(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount")!;
    }

    public static string GoogleClientId(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Google:ClientId")!;
    }

    public static string GoogleClientSecrety(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Google:ClientSecret")!;
    }

    public static string RedisConnectionString(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Redis:ConnectionString")!;
    }

    public static string PostmarkApiKey(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Postmark:ApiKey")!;
    }
}
