using Microsoft.Extensions.Configuration;

using MyRecipeBook.Domain.Services.Email;

using PostmarkDotNet;

namespace MyRecipeBook.Infrastructure.Services.Email;

public class SendCodeResetPassword(PostmarkClient postmarkClient, IConfiguration configuration) : ISendCodeResetPassword
{
    private readonly PostmarkClient _postmarkClient = postmarkClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmail(Domain.Entities.User user, string code)
    {
        var message = new PostmarkMessage
        {
            To = user.Email,
            From = _configuration.GetValue<string>("Settings:Postmark:EmailAddressSender")!,
            Subject = "Reset Password",
            TextBody = "Para redefinir sua senha use o codigo...",
            HtmlBody = await HttpContent(code),
        };

        await _postmarkClient.SendMessagesAsync(message);
    }

    private static async Task<string> HttpContent(string code)
    {
        var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        var filePath = Path.Combine(currentDirectory!, @"Services\Email\Templates\SendCodeResetPassword.html");

        var body = await File.ReadAllTextAsync(filePath);

        return body.Replace("{CODE_HERE}", code);
    }
}
