using MyRecipeBook.Communication.Converters;
using MyRecipeBook.Enums;

using System.Text.Json.Serialization;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipeJson
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IList<ResponseIngredientJson> Ingredients { get; set; } = [];
    public IList<ResponseInstructionJson> Instructions { get; set; } = [];

    [JsonConverter(typeof(EnumListConverter<DishType>))]
    public IList<DishType> DishTypes { get; set; } = [];
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CookingTime? CookingTime { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Difficulty? Difficulty { get; set; }
    
    public string? ImageUrl { get; set; }
}
