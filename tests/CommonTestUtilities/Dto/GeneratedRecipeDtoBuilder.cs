using Bogus;

using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Dto;

public class GeneratedRecipeDtoBuilder
{
    public static GeneratedRecipeDto Build()
    {
        return new Faker<GeneratedRecipeDto>()
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Ingredients, f => f.Make(1, () => f.Commerce.ProductName()))
            .RuleFor(r => r.Instructions, f => f.Make(1, () => new GeneratedInstructionDto
            {
                Step = 1,
                Text = f.Lorem.Paragraph()
            }));
    }
}
