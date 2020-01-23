namespace Dfe.Spi.Common.Caching.Factories.Managers
{
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Caches;
    using Dfe.Spi.Common.Caching.Definitions.Factories.Managers;
    using Dfe.Spi.Common.Logging.Definitions;

    /// <summary>
    /// Implements
    /// <see cref="ICacheManagerFactory{TCacheKey, TManagerItem}" />.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public abstract class CacheManagerFactory<TCacheKey, TManagerItem>
        : ICacheManagerFactory<TCacheKey, TManagerItem>
        where TManagerItem : class
    {
        private readonly ICacheManagerFactory<TCacheKey, TManagerItem> cacheManagerFactory;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="CacheManagerFactory{TCacheKey, TManagerItem}" />
        /// class.
        /// </summary>
        /// <param name="cacheManagerFactory">
        /// An instance of type
        /// <see cref="IMemoryCacheProvider{TCacheKey, TCacheValue}" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public CacheManagerFactory(
            ICacheManagerFactory<TCacheKey, TManagerItem> cacheManagerFactory,
            ILoggerWrapper loggerWrapper)
        {
            this.cacheManagerFactory = cacheManagerFactory;
            this.loggerWrapper = loggerWrapper;
        }

        /// <inheritdoc />
        public abstract Task<TManagerItem> InitialiseCacheItemAsync(
            TCacheKey cacheKey);
    }
}