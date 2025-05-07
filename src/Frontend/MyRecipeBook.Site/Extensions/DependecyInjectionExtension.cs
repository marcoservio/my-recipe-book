using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MyRecipeBook.Site.Handlers;
using MyRecipeBook.Site.Services;
using MyRecipeBook.Site.Services.Authentication;

namespace MyRecipeBook.Site.Extensions;

public static class DependecyInjectionExtension
{
    public static void AddLocalStorage(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddTransient<AuthHeaderHandler>();
    }

    public static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.AddRefitClientCustom<ILoginService>();
        builder.AddRefitClientCustom<IUserService>();
    }
}
