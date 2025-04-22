using Microsoft.OpenApi.Models;

using MyRecipeBook.CrossCutting.Binders;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyRecipeBook.CrossCutting.Filters;

public class IdsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var encryptedIds = context
            .ApiDescription
            .ParameterDescriptions
            .Where(p => p.ModelMetadata?.BinderType == typeof(MyRecipeBookIdBinder))
            .ToDictionary(p => p.Name);

        foreach (var parameter in operation.Parameters)
        {
            if (encryptedIds.TryGetValue(parameter.Name, out _))
            {
                parameter.Schema.Type = "string";
                parameter.Schema.Format = string.Empty;
            }
        }

        foreach (var schema in context.SchemaRepository.Schemas.Values)
        {
            foreach (var property in schema.Properties)
            {
                if (encryptedIds.TryGetValue(property.Key, out _))
                {
                    property.Value.Type = "string";
                    property.Value.Format = string.Empty;
                }
            }
        }
    }
}
