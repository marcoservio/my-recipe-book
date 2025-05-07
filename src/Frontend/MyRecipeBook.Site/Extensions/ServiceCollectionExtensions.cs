using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MyRecipeBook.Site.Handlers;

using Refit;

namespace MyRecipeBook.Site.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRefitClientCustom<T>(this WebAssemblyHostBuilder builder) where T : class
    {
        var url = builder.HostEnvironment.BaseAddress + builder.Configuration.BackendUrl();
        Console.WriteLine(url);

        builder.Services.AddRefitClient<T>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.BackendUrl()))
            .AddHttpMessageHandler<AuthHeaderHandler>();

    }
}
