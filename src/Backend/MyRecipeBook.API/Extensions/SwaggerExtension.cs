using Microsoft.OpenApi.Models;

using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Extensions;

public static class SwaggerExtension
{
    const string AUTHENTICATION_TYPE = "Bearer";

    public static void AddSwagger(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
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
}
