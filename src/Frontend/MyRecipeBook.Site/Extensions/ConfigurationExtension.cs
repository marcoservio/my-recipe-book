namespace MyRecipeBook.Site.Extensions;

public static class ConfigurationExtension
{
    public static string BackendUrl(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Backend:BackendUrl")!;
    }
}
