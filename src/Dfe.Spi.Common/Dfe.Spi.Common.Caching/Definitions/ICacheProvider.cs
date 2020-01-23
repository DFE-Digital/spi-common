namespace Dfe.Spi.Common.Caching.Definitions
{
    /// <summary>
    /// Describes the operations of the cache provider.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="cacheItem">
        /// The <see cref="object" /> being cached.
        /// </param>
        void AddCacheItem(string key, object cacheItem);

        /// <summary>
        /// Gets an item from the cache, unless not found, in which case null.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The cached <see cref="object" />.
        /// </returns>
        object GetCacheItem(string key);
    }
}