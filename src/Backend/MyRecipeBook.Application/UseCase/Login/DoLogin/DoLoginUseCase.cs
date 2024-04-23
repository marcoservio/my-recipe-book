using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncripter passwordEncripter, 
    IAccessTokenGenerator accessTokenGenerator) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        Validate(request);

        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }

    private static void Validate(RequestLoginJson request)
    {
        var validator = new DoLoginValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
