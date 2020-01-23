namespace Dfe.Spi.Common.Caching
{
    using System.Collections.Generic;
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
        public void AddCacheItem(string key, object cacheItem)
        {
            // We should never need to overwrite what's in the cache.
            if (!this.cache.ContainsKey(key))
            {
                // ... so just check that the key doesn't exist.
                this.cache.Add(key, cacheItem);
            }
        }

        /// <inheritdoc />
        public object GetCacheItem(string key)
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