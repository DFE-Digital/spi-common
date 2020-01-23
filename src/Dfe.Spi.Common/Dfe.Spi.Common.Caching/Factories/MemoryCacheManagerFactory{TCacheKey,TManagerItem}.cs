namespace Dfe.Spi.Common.Caching.Factories
{
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Caches;
    using Dfe.Spi.Common.Caching.Definitions.Factories;
    using Dfe.Spi.Common.Caching.Definitions.Managers;
    using Dfe.Spi.Common.Caching.Managers;
    using Dfe.Spi.Common.Logging.Definitions;

    /// <summary>
    /// Implements
    /// <see cref="IMemoryCacheManagerFactory{TCacheKey, TManagerItem}" />.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public abstract class MemoryCacheManagerFactory<TCacheKey, TManagerItem>
        : IMemoryCacheManagerFactory<TCacheKey, TManagerItem>
        where TManagerItem : class
    {
        private readonly IMemoryCacheProvider<TCacheKey, TManagerItem> memoryCacheProvider;
        private readonly ILoggerWrapper loggerWrapper;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MemoryCacheManagerFactory{TCacheKey, TManagerItem}" />
        /// class.
        /// </summary>
        /// <param name="memoryCacheProvider">
        /// An instance of type
        /// <see cref="IMemoryCacheProvider{TCacheKey, TCacheValue}" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        public MemoryCacheManagerFactory(
            IMemoryCacheProvider<TCacheKey, TManagerItem> memoryCacheProvider,
            ILoggerWrapper loggerWrapper)
        {
            this.memoryCacheProvider = memoryCacheProvider;
            this.loggerWrapper = loggerWrapper;
        }

        /// <inheritdoc />
        public IMemoryCacheManager<TCacheKey, TManagerItem> Create()
        {
            MemoryCacheManager<TCacheKey, TManagerItem> toReturn =
                new MemoryCacheManager<TCacheKey, TManagerItem>(
                    this.memoryCacheProvider,
                    this.loggerWrapper,
                    this.InitialiseCacheItemAsync);

            return toReturn;
        }

        /// <inheritdoc />
        public abstract Task<TManagerItem> InitialiseCacheItemAsync(
            TCacheKey cacheKey);
    }
}