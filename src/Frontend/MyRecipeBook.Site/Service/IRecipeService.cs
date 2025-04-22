using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

using Refit;

namespace MyRecipeBook.Site.Service;

public interface IRecipeService
{
    [Post("/recipes")]
    Task<ResponseRegisteredRecipeJson> Register(RequestRegisterRecipeFormData request);

    [Post("/recipes/filter")]
    Task<ResponseRecipesJson> Filter(RequestFilterRecipeJson request);

    [Get("/recipes/{id}")]
    Task<ResponseRecipeJson> GetById(long id);

    [Delete("/recipes/{id}")]
    Task Delete(long id);

    [Put("/recipes/{id}")]
    Task Update(long id, RequestRecipeJson request);

    [Post("/recipes/generate")]
    Task<ResponseGeneratedRecipeJson> Generate(RequestGenerateRecipeJson request);

    [Post("/recipes/image/{id}")]
    Task UpdateImage(long id, IFormFile file);
}
