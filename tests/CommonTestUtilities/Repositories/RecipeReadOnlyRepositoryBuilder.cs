﻿using Moq;

using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    public RecipeReadOnlyRepositoryBuilder() => _repository = new Mock<IRecipeReadOnlyRepository>();

    public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _repository.Setup(r => r.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);

        return this;
    }

    public RecipeReadOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
    {
        if (recipe is not null)
            _repository.Setup(r => r.GetById(user, recipe.Id)).ReturnsAsync(recipe);

        return this;
    }

    public RecipeReadOnlyRepositoryBuilder GetForDashboard(User user, IList<Recipe> recipes)
    {
        _repository.Setup(r => r.GetForDashboard(user)).ReturnsAsync(recipes);

        return this;
    }

    public IRecipeReadOnlyRepository Build() => _repository.Object;
}
