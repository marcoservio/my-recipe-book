using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Enums;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;
public class GenerateRecipeUseCase(IGenerateRecipeAI generator) : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAI _generator = generator;

    public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        Validate(request);

        var response = await _generator.Generate(request.Ingredients);

        return new ResponseGeneratedRecipeJson
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            CookingTime = response.CookingTime,
            Instructions = [.. response.Instructions.Select(i => new ResponseGeneratedInstructionJson
            {
                Step = i.Step,
                Text = i.Text,
            })],
            Difficulty = Difficulty.Low
        };
    }

    private static void Validate(RequestGenerateRecipeJson request)
    {
        var result = new GenerateRecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
    }
}
