namespace Dfe.Spi.Common.Caching.Managers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Caches;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Logging.Definitions;

    /// <summary>
    /// Implements <see cref="IMemoryCacheManager{TCacheKey, TManagerItem}" />.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public class MemoryCacheManager<TCacheKey, TManagerItem>
        : IMemoryCacheManager<TCacheKey, TManagerItem>
        where TManagerItem : class
    {
        private readonly IMemoryCacheProvider<TCacheKey, TManagerItem> memoryCacheProvider;
        private readonly ILoggerWrapper loggerWrapper;

        private readonly InitialiseCacheItemAsync initialiseCacheItemAsync;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MemoryCacheManager{TCacheKey, TCacheValue}" /> class.
        /// </summary>
        /// <param name="memoryCacheProvider">
        /// An instance of type
        /// <see cref="IMemoryCacheProvider{TCacheKey, TManagerItem}" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        /// <param name="initialiseCacheItemAsync">
        /// An instance of <see cref="InitialiseCacheItemAsync" />.
        /// </param>
        public MemoryCacheManager(
            IMemoryCacheProvider<TCacheKey, TManagerItem> memoryCacheProvider,
            ILoggerWrapper loggerWrapper,
            InitialiseCacheItemAsync initialiseCacheItemAsync)
        {
            this.memoryCacheProvider = memoryCacheProvider;
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
        /// An instance of type
        /// <typeparamref name="TManagerItem" />.
        /// </returns>
        public delegate Task<TManagerItem> InitialiseCacheItemAsync(
            TCacheKey key,
            CancellationToken cancellationToken);

        /// <inheritdoc />
        public async Task<TManagerItem> GetAsync(
            TCacheKey key,
            CancellationToken cancellationToken)
        {
            TManagerItem toReturn = null;

            string typeName = typeof(TManagerItem).Name;

            this.loggerWrapper.Debug(
                $"Checking the cache for an instance of {typeName} for " +
                $"{nameof(key)} \"{key}\"...");

            toReturn = this.memoryCacheProvider.GetCacheItem(key);

            if (toReturn == null)
            {
                this.loggerWrapper.Info(
                    $"No {typeName} found in cache with {nameof(key)} " +
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

                    this.memoryCacheProvider.AddCacheItem(key, toReturn);

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
                    $"{typeName} found in the cache for {nameof(key)} " +
                    $"\"{key}\": {toReturn}.");
            }

            return toReturn;
        }
    }
}