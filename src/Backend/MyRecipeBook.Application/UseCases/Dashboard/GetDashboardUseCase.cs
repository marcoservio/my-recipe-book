using AutoMapper;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.Caching;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public class GetDashboardUseCase(IRecipeReadOnlyRepository readOnlyRepository, IMapper mapper, ILoggedUser loggedUser, IBlobStorageService blobStorageService, ICacheService cache) : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;
    private readonly ICacheService _cache = cache;

    public async Task<ResponseRecipesJson> Execute()
    {
        var authenticatedUser = await _loggedUser.User();

        var cacheKey = $"dashboard-recipes-{authenticatedUser.UserIdentifier}";

        var recipes = await _cache.GetAsync<IList<Domain.Entities.Recipe>>(cacheKey);

        if(recipes is null)
        {
            recipes = await _readOnlyRepository.GetForDashboard(authenticatedUser);

            await _cache.SetAsync(cacheKey, recipes);
        }

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MapToShortRecipeJson(authenticatedUser, _blobStorageService, _mapper)
        };
    }
}
