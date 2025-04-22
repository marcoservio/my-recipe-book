using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestStringGenerator
{
    public static string Paragraphs(int minCharacteres)
    {
        var faker = new Faker();

        var longText = faker.Lorem.Paragraphs(count: 7);

        while(longText.Length < minCharacteres)
            longText = $"{longText} {faker.Lorem.Paragraphs()}";

        return longText;
    }
}
