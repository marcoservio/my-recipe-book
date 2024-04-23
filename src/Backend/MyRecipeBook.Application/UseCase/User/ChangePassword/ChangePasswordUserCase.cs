using FluentValidation.Results;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.ChangePassword;

public class ChangePasswordUserCase(ILoggedUser iLoggedUser, 
    IUserUpdateOnlyRepository repository, IUnitOfWork unitOfWord, 
    IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = iLoggedUser;
    private readonly IUserUpdateOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWord = unitOfWord;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWord.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncripted = _passwordEncripter.Encrypt(request.Password);

        if (currentPasswordEncripted.Equals(loggedUser.Password).IsFalse())
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_EMPTY));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
