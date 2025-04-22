using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Cache;
using CommonTestUtilities.Email;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncryption;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyRecipeBook.Enums;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MyRecipeBook.Domain.Entities.User _user = default!;
    private MyRecipeBook.Domain.Entities.Recipe _recipe = default!;
    private MyRecipeBook.Domain.Entities.CodeToPerformAction _code = default!;
    private MyRecipeBook.Domain.Entities.RefreshToken _refreshToken = default!;
    private string _password = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                StartDatabase(services);

                services.AddScoped(option => new BlobStorageServiceBuilder().Build());
                services.AddScoped(option => new CacheServiceBuilder().Build());
                services.AddScoped(option => SendCodeResetPasswordBuilder.Build());
            });
    }

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _password;
    public Guid GetUserIdentifier() => _user.UserIdentifier;

    public string GetRecipeId() => IdEncripterBuilder.Build().Encode(_recipe.Id);
    public string GetRecipeTitle() => _recipe.Title;
    public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
    public CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
    public IList<DishType> GetDishTypes() => [.. _recipe.DishTypes.Select(d => d.Type)];

    public string GetResetPasswordCode() => _code.Value;

    public string GetEnum() => nameof(CookingTime);
    public string GetEnumNotFound() => nameof(DatabaseType);

    public string GetRefreshToken() => _refreshToken.Value;

    private void StartDatabase(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

        context.Database.EnsureDeleted();

        (_user, _password) = UserBuilder.Build();

        _recipe = RecipeBuilder.Build(_user);

        _code = CodeToPerformActionBuilder.Build(_user.Id);

        _refreshToken = RefreshTokenBuilder.Build(_user);

        context.Users.Add(_user);

        context.Recipes.Add(_recipe);

        context.CodesToPerformAction.Add(_code);

        context.RefreshTokens.Add(_refreshToken);

        context.SaveChanges();
    }
}
