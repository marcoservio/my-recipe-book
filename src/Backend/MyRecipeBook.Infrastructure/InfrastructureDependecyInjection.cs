using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CodeToPerformAction;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Refresh;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.Caching;
using MyRecipeBook.Domain.Services.Email;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Enums;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Refresh;
using MyRecipeBook.Infrastructure.Security.Tokens.Validator;
using MyRecipeBook.Infrastructure.Services.Caching;
using MyRecipeBook.Infrastructure.Services.Email;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;

using OpenAI.Chat;

using PostmarkDotNet;

namespace MyRecipeBook.Infrastructure;

public static class InfrastructureDependecyInjection
{
    public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.DatabaseType() == DatabaseType.MySQL)
            AddDbContextMySqlServer(services, configuration);
        else
            AddDbContextSqlServer(services, configuration);
    }

    public static void AddDbContextMySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseMySql(configuration.ConnectionString(), new MySqlServerVersion(new Version(8, 0, 35)));
        });
    }

    public static void AddDbContextSqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionString());
        });
    }

    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();

        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();

        services.AddScoped<ICodeToPerformActionRepository, CodeToPerformActionRepository>();

        services.AddScoped<IGenerateRecipeAI, GenerateRecipeAI>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    public static void ApplyMigrations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("Migration");

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
            dbContext.Database.Migrate();
            logger.LogInformation("Migrações aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao aplicar migrações.");
        }
    }

    public static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccessTokenGenerator>(provider => new JwtTokenGenerator(
            configuration.ExpirationTimeInMinutes(),
            configuration.SigningKey()
        ));

        services.AddScoped<IAccessTokenValidator>(provider => new JwtTokenValidator(configuration.SigningKey()));

        services.AddScoped<IRefreshTokenCreator, RefreshTokenCreator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    public static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    public static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter>(option => new BCryptNet());
    }

    public static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, GenerateRecipeAI>();

        services.AddSingleton(_ => new ChatClient(
            model: configuration.OpenAIModel(),
            apiKey: configuration.OpenAIApiKey()
        ));
    }

    public static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.AzureStorageConnectionString();

        if (connectionString.NotEmpty())
        {
            services.AddScoped<IBlobStorageService>(c =>
                new AzureStorageService(new BlobServiceClient(connectionString)));
        }
    }

    public static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.AzureServiceBusConnectionString();

        if (connectionString.NotEmpty())
        {
            var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            var processor = client.CreateProcessor(nameof(ServiceBusQueue.user), new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            });

            services.AddSingleton(processor);
            services.AddSingleton<DeleteUserProcessor>();

            var deleteQueue = new DeleteUserQueue(client.CreateSender(nameof(ServiceBusQueue.user)));
            services.AddScoped<IDeleteUserQueue>(option => deleteQueue);
        }
    }

    public static void AddRedis(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheService, RedisCacheService>();

        var redisConnectionString = configuration.RedisConnectionString();

        if (redisConnectionString.NotEmpty())
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
    }

    public static void AddEmailSender(IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration.PostmarkApiKey();

        if (apiKey.NotEmpty())
        {
            var postmarkClient = new PostmarkClient(apiKey);
            var sendCodeReset = new SendCodeResetPassword(postmarkClient, configuration);

            services.AddScoped<ISendCodeResetPassword>(option => sendCodeReset);
        }
    }
}
