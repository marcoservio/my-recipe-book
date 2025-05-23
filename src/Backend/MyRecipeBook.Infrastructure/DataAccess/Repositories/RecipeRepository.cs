﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyRecipeBookDbContext context) : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Add(Recipe recipe) => await _context.Recipes.AddAsync(recipe);

    public async Task Delete(long recipeId)
    {
        var recipe = await _context.Recipes.FindAsync(recipeId);

        _context.Recipes.Remove(recipe!);
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        var query = _context
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        if(filters.Difficulties.Any())
            query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));

        if (filters.CookingTimes.Any())
            query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));

        if (filters.DishTypes.Any())
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));

        if(filters.RecipeTitle_Ingredient.NotEmpty())
            query = query.Where(
                recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient) ||
                recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));

        return await query.ToListAsync();
    }

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public void Update(Recipe recipe) => _context.Recipes.Update(recipe);

    private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
    {
        return _context
            .Recipes
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes);
    }

    public async Task<IList<Recipe>> GetForDashboard(User user)
    {
        return await _context
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id)
            .OrderByDescending(recipe => recipe.CreatedOn)
            .Take(5)
            .ToListAsync();
    }
}
