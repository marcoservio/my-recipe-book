using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using MyRecipeBook.Application;
using MyRecipeBook.CrossCutting.BackgroudServices;
using MyRecipeBook.CrossCutting.Converters;
using MyRecipeBook.CrossCutting.Filters;
using MyRecipeBook.CrossCutting.Middleware;
using MyRecipeBook.CrossCutting.Token;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.Extensions;

using Serilog;

using System.Text.Json;
using System.Threading.RateLimiting;

namespace MyRecipeBook.CrossCutting;

public static class DependecyInjection
{
    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void AddHostedServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        services.AddHostedService<DeleteUserService>();
    }

    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                }));
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.RetryAfter = "60 seconds";
                await context.HttpContext.Response.WriteAsync("Too Many Requests", token);
            };
        });
    }

    public static void AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        const string AUTHENTICATION_TYPE = "Bearer";

        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<IdsFilter>();

            options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = AUTHENTICATION_TYPE
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = AUTHENTICATION_TYPE
                        },
                        Scheme = "oauth2",
                        Name = AUTHENTICATION_TYPE,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        ApplicationDependecyInjection.AddAutoMapper(services);
        ApplicationDependecyInjection.AddIdEncoder(services, configuration);
        ApplicationDependecyInjection.AddUseCases(services);
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        InfrastructureDependecyInjection.AddPasswordEncripter(services, configuration);
        InfrastructureDependecyInjection.AddRepositories(services);
        InfrastructureDependecyInjection.AddLoggedUser(services);
        InfrastructureDependecyInjection.AddTokens(services, configuration);

        if (configuration.IsUnitTestEnviroment())
            return;

        InfrastructureDependecyInjection.AddDbContext(services, configuration);
        InfrastructureDependecyInjection.AddOpenAI(services, configuration);
        InfrastructureDependecyInjection.AddAzureStorage(services, configuration);
        InfrastructureDependecyInjection.AddQueue(services, configuration);
        InfrastructureDependecyInjection.AddRedis(services, configuration);
        InfrastructureDependecyInjection.AddEmailSender(services, configuration);
    }

    public static void AddTokenProvider(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, HttpContextTokenValue>();

        services.AddHttpContextAccessor();
    }

    public static void AddFilters(this IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });
    }

    public static void AddMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<CultureMiddleware>();
    }

    public static void EnsureDatabaseMigrated(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        InfrastructureDependecyInjection.ApplyMigrations(app.ApplicationServices);
    }

    public static void UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);
    }

    public static void AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration.GoogleClientId();
            googleOptions.ClientSecret = configuration.GoogleClientSecrety();
        });
    }

    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
        });
    }

    public static void AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks().AddDbContextCheck<MyRecipeBookDbContext>();
    }

    public static void AddHealthCheck(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
    }
}
