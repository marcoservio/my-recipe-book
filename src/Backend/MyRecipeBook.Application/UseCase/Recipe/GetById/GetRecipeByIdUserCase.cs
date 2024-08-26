using AutoMapper;

using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.Recipe.GetById;

public class GetRecipeByIdUserCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IBlobStorageService blobStorageService) : IGetRecipeByIdUserCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _repository = repository;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var logged = await _loggedUser.User();

        var recipe = await _repository.GetById(logged, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var response = _mapper.Map<ResponseRecipeJson>(recipe);

        if (recipe.ImageIdentifier.NotEmpty())
        {
            var url = await _blobStorageService.GetFileUrl(logged, recipe.ImageIdentifier);

            response.ImageUrl = url;
        }

        return response;
    }
}
