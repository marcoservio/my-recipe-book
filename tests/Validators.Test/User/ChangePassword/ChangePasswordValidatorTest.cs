using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = new ChangePasswordValidator().Validate(request);

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
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        var result = new ChangePasswordValidator().Validate(request);

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
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password!;

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.PASSWORD_EMPTY);
    }
}
