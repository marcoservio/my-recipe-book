using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.Recipe.Delete;

public class DeleteRecipeUseCase(
    ILoggedUser loggedUser,
    IRecipeReadOnlyRepository repositoryRead,
    IRecipeWriteOnlyRepository repositoryWrite,
    IUnitOfWork unitOfWord) : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeReadOnlyRepository _repositoryRead = repositoryRead;
    private readonly IRecipeWriteOnlyRepository _repositoryWrite = repositoryWrite;
    private readonly IUnitOfWork _unitOfWord = unitOfWord;

    public async Task Execute(long id)
    {
        var logged = await _loggedUser.User();

        var _ = await _repositoryRead.GetById(logged, id) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        await _repositoryWrite.Delete(id);

        await _unitOfWord.Commit();
    }
}
