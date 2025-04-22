using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyRecipeBook.Communication.Converters;

public class EnumListConverter<TEnum> : JsonConverter<IList<TEnum>> where TEnum : struct, Enum
{
    public override IList<TEnum> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException($"Expected an array for {typeof(TEnum).Name} list.");

        var values = new List<TEnum>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Expected a string value for {typeof(TEnum).Name}.");

            var enumString = reader.GetString();
            if (Enum.TryParse(enumString, true, out TEnum enumValue))
            {
                values.Add(enumValue);
            }
            else
            {
                throw new JsonException($"Invalid value '{enumString}' for enum {typeof(TEnum).Name}.");
            }
        }

        return values;
    }

    public override void Write(Utf8JsonWriter writer, IList<TEnum> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStringValue(item.ToString());
        }
        writer.WriteEndArray();
    }
}

