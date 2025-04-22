using AutoMapper;

using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.Caching;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository readOnlyRepository, IBlobStorageService blobStorageService, ICacheService cache) : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;
    private readonly ICacheService _cache = cache;

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var authenticatedUser = await _loggedUser.User();

        var cacheKey = $"getById-recipe-{recipeId}-{authenticatedUser.UserIdentifier}";

        var recipe = await _cache.GetAsync<Domain.Entities.Recipe>(cacheKey);

        if(recipe is null)
        {
            recipe = await _readOnlyRepository.GetById(authenticatedUser, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            await _cache.SetAsync(cacheKey, recipe);
        }

        var response = _mapper.Map<ResponseRecipeJson>(recipe);

        if (recipe!.ImageIdentifier.NotEmpty())
        {
            var url = await _blobStorageService.GetFileUrl(authenticatedUser, recipe.ImageIdentifier!);

            response.ImageUrl = url;
        }

        return response;
    }
}
