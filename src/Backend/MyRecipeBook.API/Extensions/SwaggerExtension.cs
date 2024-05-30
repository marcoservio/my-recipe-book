using Microsoft.OpenApi.Models;

using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Extensions;

public static class SwaggerExtension
{
    const string BearerScheme = "Bearer";

    public static void AddSwagger(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {
            options.OperationFilter<IdsFilter>();

            options.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = BearerScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = BearerScheme
                        },
                        Scheme = "oauth2",
                        Name = BearerScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}
