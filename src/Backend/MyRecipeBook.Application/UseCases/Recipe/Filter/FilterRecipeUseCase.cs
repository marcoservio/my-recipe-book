using AutoMapper;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.Caching;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository readOnlyRepository, IBlobStorageService blobStorageService, ICacheService cache) : IFilterRecipeUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;
    private readonly ICacheService _cache = cache;

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);

        var authenticatedUser = await _loggedUser.User();

        var filters = new Domain.Dtos.FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = [.. request.CookingTimes.Distinct()],
            Difficulties = [.. request.Difficulties.Distinct()],
            DishTypes = [.. request.DishTypes.Distinct()]
        };

        var cacheKey = $"filter-recipes-{authenticatedUser.UserIdentifier}-" +
               $"RecipeTitle_Ingredient={filters.RecipeTitle_Ingredient}-" +
               $"CookingTimes=[{string.Join(",", filters.CookingTimes)}]-" +
               $"Difficulties=[{string.Join(",", filters.Difficulties)}]-" +
               $"DishTypes=[{string.Join(",", filters.DishTypes)}]";

        var recipes = await _cache.GetAsync<IList<Domain.Entities.Recipe>>(cacheKey);

        if (recipes is null)
        {
            recipes = await _readOnlyRepository.Filter(authenticatedUser, filters);

            await _cache.SetAsync(cacheKey, recipes);
        }

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MapToShortRecipeJson(authenticatedUser, _blobStorageService, _mapper)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var result = new FilterRecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(e => e.ErrorMessage).Distinct()]);
    }
}
