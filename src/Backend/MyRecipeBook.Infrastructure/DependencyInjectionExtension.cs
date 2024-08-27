using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;

using FluentMigrator.Runner;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;

using OpenAI_API;

using System.Reflection;

namespace MyRecipeBook.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncripter(services);
        AddRepository(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        AddDbContext(services, configuration);
        AddOpenAI(services, configuration);
        AddAzureStorage(services, configuration);
        AddQueue(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.MySQL)
        {
            AddDbContextMySqlServer(services, configuration);
            AddFluentMigratorMySQL(services, configuration);
        }
        else
        {
            AddDbContextSqlServer(services, configuration);
            AddFluentMigratorSQLServer(services, configuration);
        }
    }

    private static void AddDbContextMySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var serverVersion = new MySqlServerVersion(new Version(5, 7, 44));

        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseMySql(configuration.ConnectionString(), serverVersion);
        });
    }

    private static void AddDbContextSqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionString());
        });
    }

    private static void AddRepository(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();

        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
    }

    private static void AddFluentMigratorMySQL(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddMySql5()
            .WithGlobalConnectionString(configuration.ConnectionString())
            .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddFluentMigratorSQLServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(configuration.ConnectionString())
            .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddPasswordEncripter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGptService>();

        var key = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

        var authentication = new APIAuthentication(key);

        services.AddScoped<IOpenAIAPI>(option => new OpenAIAPI(authentication));
    }

    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (connectionString.NotEmpty())
            services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
    }

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        //var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

        //var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions()
        //{
        //    TransportType = ServiceBusTransportType.AmqpWebSockets
        //});

        //var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));

        //var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        //{
        //    MaxConcurrentCalls = 1
        //}));

        //services.AddSingleton(deleteUserProcessor);

        //services.AddScoped<IDeleteUserQueue>(options => deleteQueue);

        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

        if (connectionString.NotEmpty())
        {
            services.AddSingleton(serviceProvider =>
            {
                var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                });

                return client;
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<ServiceBusClient>();
                return client.CreateProcessor("user", new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 1
                });
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<ServiceBusClient>();
                return new DeleteUserQueue(client.CreateSender("user"));
            });

            services.AddScoped<IDeleteUserQueue>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<DeleteUserQueue>();
            });
        }
    }
}
