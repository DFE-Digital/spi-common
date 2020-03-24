using System;

namespace Dfe.Spi.Common.Caching.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;

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
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task AddCacheItemAsync(
            string key,
            object cacheItem,
            CancellationToken cancellationToken);
        
        /// <summary>
        /// Adds an item to the cache that will expire after a given duration
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="cacheItem">
        /// The <see cref="object" /> being cached.
        /// </param>
        /// <param name="timeToLive">
        /// The duration the item should live in cache until it expires
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns></returns>
        Task AddCacheItemAsync(
            string key,
            object cacheItem,
            TimeSpan timeToLive,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets an item from the cache, unless not found, in which case null.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// The cached <see cref="object" />.
        /// </returns>
        Task<object> GetCacheItemAsync(
            string key,
            CancellationToken cancellationToken);
    }
}