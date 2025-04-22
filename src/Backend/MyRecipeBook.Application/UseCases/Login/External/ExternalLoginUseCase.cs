using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCases.Login.External;

public class ExternalLoginUseCase(IUserWriteOnlyRepository writeOnlyRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator, IUserReadOnlyRepository readOnlyRepository) : IExternalLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<string> Execute(string name, string email)
    {
        var user = await _readOnlyRepository.GetByEmail(email);

        if(user is null)
        {
            user = new Domain.Entities.User
            {
                Name = name,
                Email = email,
                Password = "-"
            };

            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();
        }

        return _accessTokenGenerator.Generate(user.UserIdentifier);
    }
}
