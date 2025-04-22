using Microsoft.AspNetCore.Mvc.ModelBinding;

using MyRecipeBook.Domain.Extensions;

using Sqids;

namespace MyRecipeBook.CrossCutting.Binders;

public class MyRecipeBookIdBinder(SqidsEncoder<long> idEncoder) : IModelBinder
{
    private readonly SqidsEncoder<long> _idEncoder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (value.Empty())
            return Task.CompletedTask;

        var id = _idEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
