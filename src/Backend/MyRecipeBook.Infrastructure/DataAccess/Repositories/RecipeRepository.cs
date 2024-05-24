using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyRecipeBookDbContext context) : IRecipeWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }
}
