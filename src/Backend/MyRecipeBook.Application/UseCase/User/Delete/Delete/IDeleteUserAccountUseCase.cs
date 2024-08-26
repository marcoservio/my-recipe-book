namespace MyRecipeBook.Application.UseCase.User.Delete.Delete;

public interface IDeleteUserAccountUseCase
{
    Task Execute(Guid userIdentifier);
}
