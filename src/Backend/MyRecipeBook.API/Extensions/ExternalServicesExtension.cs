using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.API.Extensions;

public static class ExternalServicesExtension
{
    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment().IsFalse())
            services.AddGoogleAuthentication(configuration);
    }
}
