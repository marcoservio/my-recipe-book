using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCase.Login.DoLogin;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Exceptions;

using Xunit;

namespace Validators.Test.Login.DoLogin;

public class DoLoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new DoLoginValidator();
        var request = RequestLoginJsonBuilder.Build();
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new DoLoginValidator();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new DoLoginValidator();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = "emailinvalid";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new DoLoginValidator();
        var request = RequestLoginJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_INVALID));
    }
}
