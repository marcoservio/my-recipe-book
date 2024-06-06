using AutoMapper;

using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCase.Dashboard;

public class GetDashboardUseCase(IRecipeReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseRecipesJson> Execute()
    {
        var logged = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(logged);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes),
        };
    }
}
