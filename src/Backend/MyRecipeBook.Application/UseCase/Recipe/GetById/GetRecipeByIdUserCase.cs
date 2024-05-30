using AutoMapper;

using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.Recipe.GetById;

public class GetRecipeByIdUserCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository) : IGetRecipeByIdUserCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _repository = repository;

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var logged = await _loggedUser.User();

        var recipe = await _repository.GetById(logged, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}
