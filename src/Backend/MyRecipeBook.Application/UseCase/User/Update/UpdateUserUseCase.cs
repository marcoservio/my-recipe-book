using AutoMapper;

using FluentValidation.Results;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.Update;

public class UpdateUserUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateOnlyrepository,
    IUserReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper) : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyrepository = updateOnlyrepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser =  await _loggedUser.User();

        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyrepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyrepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
