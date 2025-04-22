using FluentValidation.Results;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task Execute(RequestChangePasswordJson request)
    {
        var authenticatedUser = await _loggedUser.User();

        Validate(request, authenticatedUser);

        var user = await _updateOnlyRepository.GetById(authenticatedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User authenticatedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordEncripter.IsValid(request.Password, authenticatedUser.Password).IsFalse())
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if(result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
    }
}
