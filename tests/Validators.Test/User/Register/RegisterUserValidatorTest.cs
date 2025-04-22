using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Name_Empty(string? name)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_EMPTY);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "emailInvalid";

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_INVALID);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = new RegisterUserValidator().Validate(request);

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
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.PASSWORD_EMPTY);
    }
}
