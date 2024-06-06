using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.Dashboard;

public interface IGetDashboardUseCase
{
    Task<ResponseRecipesJson> Execute();
}
