using MyRecipeBook.API.BackgroundServices;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.API.Extensions;

public static class BackgroupServicesExtension
{
    public static void AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment().IsFalse())
            services.AddHostedService<DeleteUserService>();
    }
}
