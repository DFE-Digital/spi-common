namespace Dfe.Spi.Common.Caching.Managers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Logging.Definitions;

    /// <summary>
    /// Implements <see cref="ICacheManager" />. An in-memory implementation of
    /// <see cref="ICacheManager" />.
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly ICacheProvider cacheProvider;
        private readonly ILoggerWrapper loggerWrapper;
        private readonly InitialiseCacheItemAsync initialiseCacheItemAsync;

        /// <summary>
        /// Initialises a new instance of the <see cref="CacheManager" />
        /// class.
        /// </summary>
        /// <param name="cacheProvider">
        /// An instance of type <see cref="ICacheProvider" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="initialiseCacheItemAsync">
        /// An instance of <see cref="InitialiseCacheItemAsync" />.
        /// </param>
        public CacheManager(
            ICacheProvider cacheProvider,
            ILoggerWrapper loggerWrapper,
            InitialiseCacheItemAsync initialiseCacheItemAsync)
        {
            this.cacheProvider = cacheProvider;
            this.loggerWrapper = loggerWrapper;
            this.initialiseCacheItemAsync = initialiseCacheItemAsync;
        }

        /// <summary>
        /// Initialises an item if it cannot be found in the cache.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// The item to cache, as an <see cref="object" />.
        /// </returns>
        public delegate Task<object> InitialiseCacheItemAsync(
            string key,
            CancellationToken cancellationToken);

        /// <inheritdoc />
        public async Task<object> GetAsync(
            string key,
            CancellationToken cancellationToken)
        {
            object toReturn = null;

            this.loggerWrapper.Debug(
                $"Checking the cache for an enty with {nameof(key)} " +
                $"\"{key}\"...");

            toReturn = this.cacheProvider.GetCacheItem(key);

            if (toReturn == null)
            {
                this.loggerWrapper.Info(
                    $"No entry found in cache with {nameof(key)} " +
                    $"\"{key}\". Attempting to initialise a value for this " +
                    $"key...");

                toReturn = await this.initialiseCacheItemAsync(
                    key,
                    cancellationToken)
                    .ConfigureAwait(false);

                if (toReturn != null)
                {
                    this.loggerWrapper.Debug(
                        $"Storing {toReturn} in cache with {nameof(key)} " +
                        $"\"{key}\"...");

                    this.cacheProvider.AddCacheItem(key, toReturn);

                    this.loggerWrapper.Info(
                        $"{toReturn} stored in cache with {nameof(key)} " +
                        $"\"{key}\".");
                }
                else
                {
                    this.loggerWrapper.Warning(
                        $"The manager could not initialise a value for key " +
                        $"\"{key}\"!");
                }
            }
            else
            {
                this.loggerWrapper.Debug(
                    $"Entry found in the cache for {nameof(key)} \"{key}\": " +
                    $"{toReturn}.");
            }

            return toReturn;
        }
    }
}