﻿using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncryption;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MyRecipeBook.Domain.Entities.Recipe _recipe = default!;
    private MyRecipeBook.Domain.Entities.User _user = default!;
    private string _password = string.Empty;
    private MyRecipeBook.Domain.Entities.RefreshToken _refreshToken = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services
                .SingleOrDefault(d => d.ServiceType.Equals(typeof(DbContextOptions<MyRecipeBookDbContext>)));

                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                var blobStorage = new BlobStorageServiceBuilder().Build();
                services.AddScoped(option => blobStorage);

                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetService<MyRecipeBookDbContext>();

                dbContext!.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }

    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    public string GetRefreshToken() => _refreshToken.Value;

    public string GetRecipeId() => IdEncripterBuilder.Build().Encode(_recipe.Id);
    public string GetRecipeTitle() => _recipe.Title;
    public MyRecipeBook.Domain.Enums.Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
    public MyRecipeBook.Domain.Enums.CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
    public IList<MyRecipeBook.Domain.Enums.DishType> GetDishType() => _recipe.DishTypes.Select(c => c.Type).ToList();

    private void StartDatabase(MyRecipeBookDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        _recipe = RecipeBuilder.Build(_user);

        _refreshToken = RefreshTokenBuilder.Build(_user);

        dbContext.Users.Add(_user);

        dbContext.Recipes.Add(_recipe);

        dbContext.RefreshTokens.Add(_refreshToken);

        dbContext.SaveChanges();
    }
}
