using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

using FluentAssertions;

using Microsoft.AspNetCore.Http;

using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image;

public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success_With_ImageIdentifier_Empty(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);
        recipe.ImageIdentifier = string.Empty;

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Error_Recipe_Not_Found(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(1, file);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var file = FormFileBuilder.Txt();

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        (await act.Should().ThrowAsync<OnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
    }

    private static AddUpdateImageCoverUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var updateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new AddUpdateImageCoverUseCase(loggedUser, updateOnlyRepository, unitOfWork, blobStorage);
    }
}
