using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyRecipeBook.API.Extensions;

public static class GoogleAuthenticationExtension
{
    public static void AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var clientId = configuration.GetValue<string>("Settings:Google:ClientId")!;
        var clientSecret = configuration.GetValue<string>("Settings:Google:ClientSecret")!;

        services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = clientId;
            googleOptions.ClientSecret = clientSecret;
        });
    }
}
