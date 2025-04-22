using FluentValidation.Results;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateOnlyRepository, IUserReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestUpdateUserJson request)
    {
        var authenticatedUser = await _loggedUser.User();

        await Validate(request, authenticatedUser.Email);

        var user = await _updateOnlyRepository.GetById(authenticatedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if (currentEmail.Equals(request.Email).IsFalse() && await _readOnlyRepository.ExistActiveUserWithEmail(request.Email))
            result.Errors.Add(new ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
    }
}
