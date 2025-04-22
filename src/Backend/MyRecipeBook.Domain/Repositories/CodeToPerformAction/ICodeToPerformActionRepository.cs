namespace MyRecipeBook.Domain.Repositories.CodeToPerformAction;

public interface ICodeToPerformActionRepository
{
    Task Add(Entities.CodeToPerformAction code);
    Task<Entities.CodeToPerformAction?> GetByCode(string code);
    Task DeleteAllUserCodes(long userId);
}
