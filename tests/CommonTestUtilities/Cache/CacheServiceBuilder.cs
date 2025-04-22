using Moq;

using MyRecipeBook.Domain.Services.Caching;

namespace CommonTestUtilities.Cache;

public class CacheServiceBuilder
{
    private readonly Mock<ICacheService> _cache;

    public CacheServiceBuilder() => _cache = new Mock<ICacheService>();

    public CacheServiceBuilder GetAsync<T>(T obj)
    {
        _cache.Setup(x => x.GetAsync<T>(It.IsAny<string>())).ReturnsAsync(obj);
        return this;
    }

    public ICacheService Build() => _cache.Object;
}
