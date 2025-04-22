using Microsoft.Extensions.Options;

using Refit;

namespace MyRecipeBook.Site.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRefitClientCustom<T>(this IServiceCollection services) where T : class
    {
        services.AddRefitClient<T>()
            .ConfigureHttpClient((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                client.BaseAddress = new Uri(settings.BackendUrl);
            });
    }
}
