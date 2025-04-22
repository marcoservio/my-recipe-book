using FluentValidation.Results;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CodeToPerformAction;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.ResetPassword;

public class ResetPasswordUseCase(ICodeToPerformActionRepository codeRepository, IUserUpdateOnlyRepository userRepository, IPasswordEncripter passwordEncripter, IUnitOfWork unitOfWork) : IResetPasswordUseCase
{
    private readonly ICodeToPerformActionRepository _codeRepository = codeRepository;
    private readonly IUserUpdateOnlyRepository _userRepository = userRepository;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestResetYourPasswordJson request)
    {
        var code = await _codeRepository.GetByCode(request.Code) ??
            throw new OnValidationException([ResourceMessagesException.CODE_IS_NULL]);

        var user = await _userRepository.GetById(code.UserId);

        Validate(user, code, request);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _userRepository.Update(user);

        await _codeRepository.DeleteAllUserCodes(user.Id);

        await _unitOfWork.Commit();
    }

    private static void Validate(Domain.Entities.User? user, Domain.Entities.CodeToPerformAction code, RequestResetYourPasswordJson request)
    {
        if (user is null)
            throw new OnValidationException([ResourceMessagesException.INVALID_CODE]);

        if (user.Email.Equals(request.Email).IsFalse())
            throw new OnValidationException([ResourceMessagesException.INVALID_CODE]);

        var validation = new ResetPasswordValidation().Validate(request);

        if (DateTime.Compare(code.CreatedOn.AddHours(MyRecipeBookRuleConstants.PASSWORD_RESET_CODE_VALIDITY_HOURS), DateTime.UtcNow) <= 0)
            validation.Errors.Add(new ValidationFailure("ExpiredCode", ResourceMessagesException.EXPIRED_CODE));

        if (validation.IsValid.IsFalse())
            throw new OnValidationException([.. validation.Errors.Select(x => x.ErrorMessage)]);
    }
}
