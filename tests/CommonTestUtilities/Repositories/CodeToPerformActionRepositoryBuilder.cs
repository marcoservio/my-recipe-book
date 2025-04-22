using Moq;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.CodeToPerformAction;

namespace CommonTestUtilities.Repositories;

public class CodeToPerformActionRepositoryBuilder
{
    private readonly Mock<ICodeToPerformActionRepository> _repository;

    public CodeToPerformActionRepositoryBuilder() => _repository = new Mock<ICodeToPerformActionRepository>();

    public CodeToPerformActionRepositoryBuilder GetByCode(CodeToPerformAction? code)
    {
        if(code is not null)
            _repository.Setup(x => x.GetByCode(It.IsAny<string>())).ReturnsAsync(code);

        return this;
    }

    public ICodeToPerformActionRepository Build() => _repository.Object;
}
