using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncripter passwordEncripter) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        Validate(request);

        var encriptedPassword = _passwordEncripter.Encript(request.Password);
        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisterUserJson
        {
            Name = user.Name
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
