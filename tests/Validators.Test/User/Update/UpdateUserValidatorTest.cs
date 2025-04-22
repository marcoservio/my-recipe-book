using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Name_Empty(string? name)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name!;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_EMPTY); ;
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email!;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "emailInvalid";

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(x => x.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }
}
