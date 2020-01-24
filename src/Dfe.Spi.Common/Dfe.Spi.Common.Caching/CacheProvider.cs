namespace Dfe.Spi.Common.Caching
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions;

    /// <summary>
    /// Implements <see cref="ICacheProvider" />. An in-memory implementation
    /// of <see cref="ICacheProvider" />.
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private readonly Dictionary<string, object> cache;

        /// <summary>
        /// Initialises a new instance of the <see cref="CacheProvider" />
        /// class.
        /// </summary>
        public CacheProvider()
        {
            this.cache = new Dictionary<string, object>();
        }

        /// <inheritdoc />
        public Task AddCacheItemAsync(
            string key,
            object cacheItem,
            CancellationToken cancellationToken)
        {
            this.AddCacheItem(key, cacheItem);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<object> GetCacheItemAsync(
            string key,
            CancellationToken cancellationToken)
        {
            object toReturn = this.GetCacheItem(key);

            return Task.FromResult(toReturn);
        }

        private void AddCacheItem(string key, object cacheItem)
        {
            // We should never need to overwrite what's in the cache.
            if (!this.cache.ContainsKey(key))
            {
                // ... so just check that the key doesn't exist.
                this.cache.Add(key, cacheItem);
            }
        }

        private object GetCacheItem(string key)
        {
            object toReturn = null;

            if (this.cache.ContainsKey(key))
            {
                toReturn = this.cache[key];
            }

            return toReturn;
        }
    }
}