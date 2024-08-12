using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task Upload(User user, Stream stream, string fileName);
    Task<string> GetImageUrl(User user, string fileName);
}
