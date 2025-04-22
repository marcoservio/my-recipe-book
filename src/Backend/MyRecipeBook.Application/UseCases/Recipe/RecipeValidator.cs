using FluentValidation;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        RuleFor(r => r.CookingTime).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        RuleFor(r => r.Difficulty).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleFor(r => r.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
        RuleFor(r => r.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        RuleForEach(r => r.Ingredients).NotEmpty().WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);
        RuleForEach(r => r.Instructions).ChildRules(i =>
        {
            i.RuleFor(i => i.Step).GreaterThan(0).WithMessage(ResourceMessagesException.GREATER_THAN_ZERO_INSTRUCTION_STEP);
            i.RuleFor(i => i.Text)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY)
            .MaximumLength(2000)
            .WithMessage(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        });
        RuleFor(r => r.Instructions).Must(i => i.Select(i => i.Step).Distinct().Count() == i.Count)
            .WithMessage(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }
}
