using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Login.ResetPassword;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Login.ResetPassword;

public class ResetPasswordValidationTest
{
    [Fact]
    public void Success()
    {
        var request = RequestResetYourPasswordJsonBuilder.Build();

        var result = new ResetPasswordValidation().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var request = RequestResetYourPasswordJsonBuilder.Build(passwordLength: passwordLength);

        var result = new ResetPasswordValidation().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.PASSWORD_INVALID);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Password_Empty(string? password)
    {
        var request = RequestResetYourPasswordJsonBuilder.Build();
        request.NewPassword = password!;

        var result = new ResetPasswordValidation().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.PASSWORD_EMPTY);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestResetYourPasswordJsonBuilder.Build();
        request.Email = email!;

        var result = new ResetPasswordValidation().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
    }
}
