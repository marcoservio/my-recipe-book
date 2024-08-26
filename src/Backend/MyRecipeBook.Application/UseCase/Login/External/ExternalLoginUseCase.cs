using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCase.Login.External;

public class ExternalLoginUseCase(IUserReadOnlyRepository repositoryRead, IUserWriteOnlyRepository repositoryWrite, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator) : IExternalLoginUseCase
{
    private readonly IUserReadOnlyRepository _repositoryRead = repositoryRead;
    private readonly IUserWriteOnlyRepository _repositoryWrite = repositoryWrite;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<string> Execute(string name, string email)
    {
        var user = await _repositoryRead.GetByEmail(email);

        if (user is null)
        {
            user = new Domain.Entities.User()
            {
                Name = name,
                Email = email,
                Password = "-"
            };

            await _repositoryWrite.Add(user);
            await _unitOfWork.Commit();
        }

        return _accessTokenGenerator.Generate(user!.UserIdentifier);
    }
}
