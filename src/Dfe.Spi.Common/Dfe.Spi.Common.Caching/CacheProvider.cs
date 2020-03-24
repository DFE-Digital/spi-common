using System;

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
        private readonly Dictionary<string, CacheItem> cache;

        /// <summary>
        /// Initialises a new instance of the <see cref="CacheProvider" />
        /// class.
        /// </summary>
        public CacheProvider()
        {
            this.cache = new Dictionary<string, CacheItem>();
        }

        /// <inheritdoc />
        public Task AddCacheItemAsync(
            string key,
            object cacheItem,
            CancellationToken cancellationToken)
        {
            this.AddCacheItem(key, cacheItem, null);

            return Task.CompletedTask;
        }

        public Task AddCacheItemAsync(
            string key, 
            object cacheItem, 
            TimeSpan timeToLive, 
            CancellationToken cancellationToken)
        {
            this.AddCacheItem(key, cacheItem, timeToLive);

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

        private void AddCacheItem(string key, object value, TimeSpan? timeToLive)
        {
            var cacheItem = new CacheItem
            {
                Value = value,
                ExpiresAt = timeToLive.HasValue ? DateTime.UtcNow.Add(timeToLive.Value) : DateTime.MaxValue,
            };

            if (this.cache.ContainsKey(key))
            {
                this.cache[key] = cacheItem;
            }
            else
            {
                this.cache.Add(key, cacheItem);
            }
        }

        private object GetCacheItem(string key)
        {
            object toReturn = null;

            if (this.cache.ContainsKey(key))
            {
                var cacheItem = this.cache[key];
                if (DateTime.UtcNow < cacheItem.ExpiresAt)
                {
                    toReturn = cacheItem.Value;
                }
            }

            return toReturn;
        }
        
        private class CacheItem
        {
            public object Value { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}