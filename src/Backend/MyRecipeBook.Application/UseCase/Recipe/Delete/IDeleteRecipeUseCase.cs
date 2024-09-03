namespace MyRecipeBook.Application.UseCase.Recipe.Delete;

public interface IDeleteRecipeUseCase
{
    Task Execute(long id);
}
