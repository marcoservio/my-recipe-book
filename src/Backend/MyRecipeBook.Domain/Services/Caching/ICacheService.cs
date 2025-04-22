namespace MyRecipeBook.Domain.Services.Caching;

public interface ICacheService
{
    Task SetAsync<T>(string key, T obj);
    Task<T> GetAsync<T>(string key);
}
