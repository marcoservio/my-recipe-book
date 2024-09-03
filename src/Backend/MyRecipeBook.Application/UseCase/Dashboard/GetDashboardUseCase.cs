using AutoMapper;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCase.Dashboard;

public class GetDashboardUseCase(IRecipeReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser, IBlobStorageService bobStorageService) : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IBlobStorageService _bobStorageService = bobStorageService;

    public async Task<ResponseRecipesJson> Execute()
    {
        var logged = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(logged);

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MapToShortRecipeJson(logged, _bobStorageService, _mapper)
        };
    }
}
