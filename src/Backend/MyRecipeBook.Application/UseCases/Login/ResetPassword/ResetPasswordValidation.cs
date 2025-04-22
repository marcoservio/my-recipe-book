using FluentValidation;

using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Login.ResetPassword;

public class ResetPasswordValidation : AbstractValidator<RequestResetYourPasswordJson>
{
    public ResetPasswordValidation()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestResetYourPasswordJson>());
        When(user => user.Email.NotEmpty(), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}
