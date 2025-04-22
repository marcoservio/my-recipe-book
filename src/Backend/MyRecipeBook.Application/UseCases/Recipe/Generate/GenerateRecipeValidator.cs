using FluentValidation;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maximumNumberIngredients = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;

        RuleFor(r => r.Ingredients.Count).InclusiveBetween(1, maximumNumberIngredients).WithMessage(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);
        RuleFor(r => r.Ingredients).Must(i => i.Count == i.Distinct().Count()).WithMessage(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
        RuleFor(r => r.Ingredients).ForEach(rule =>
        {
            rule.Custom((value, context) =>
            {
                if (value.Empty())
                {
                    context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_EMPTY);
                    return;
                }

                if(value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                    context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
            });
        });
    }
}
