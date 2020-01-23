namespace Dfe.Spi.Common.Caching.Definitions.Caches
{
    /// <summary>
    /// Describes the operations of the memory cache provider.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TCacheValue">
    /// The type of item to store in the cache.
    /// </typeparam>
    public interface IMemoryCacheProvider<TCacheKey, TCacheValue>
        : ICacheProvider<TCacheKey, TCacheValue>
        where TCacheValue : class
    {
        // Nothing - just inherits from ICacheProvider.
    }
}