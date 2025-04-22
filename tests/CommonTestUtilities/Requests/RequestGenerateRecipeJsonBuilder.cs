using Bogus;

using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build(int count = 5)
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(r => r.Ingredients, f => f.Make(count, () => f.Commerce.ProductName()));
    }
}
