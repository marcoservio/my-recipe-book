using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Enums;

using OpenAI.Chat;

namespace MyRecipeBook.Infrastructure.Services.OpenAI;

public class GenerateRecipeAI(ChatClient chatClient) : IGenerateRecipeAI
{
    private readonly ChatClient _chatClient = chatClient;

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE),
            new UserChatMessage(string.Join(";", ingredients))
        };

        ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);

        var content = completion.Content[0].Text;

        var responseList = content
            .Split("\n")
            .Where(line => line.NotEmpty())
            .Select(line => line.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = Enum.TryParse<CookingTime>(responseList[1], out var cookingTime) ? cookingTime : default,
            Ingredients = responseList[2].Split(";"),
            Instructions = [.. responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Step = step++,
                Text = instruction.Trim()
            })]
        };
    }
}
