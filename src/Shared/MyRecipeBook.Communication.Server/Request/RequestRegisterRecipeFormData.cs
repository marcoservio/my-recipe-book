using Microsoft.AspNetCore.Http;

using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Communication.Server.Requests;

public class RequestRegisterRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}
