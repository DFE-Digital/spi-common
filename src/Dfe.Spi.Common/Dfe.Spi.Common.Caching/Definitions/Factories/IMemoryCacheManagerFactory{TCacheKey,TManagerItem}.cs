namespace Dfe.Spi.Common.Caching.Definitions.Factories
{
    using Dfe.Spi.Common.Caching.Definitions.Managers;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="IMemoryCacheManager{TCacheKey, TManagerItem}" /> factory.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public interface IMemoryCacheManagerFactory<TCacheKey, TManagerItem>
        : ICacheManagerFactory<TCacheKey, TManagerItem>
        where TManagerItem : class
    {
        /// <summary>
        /// Creates an instance of type
        /// <see cref="IMemoryCacheManager{TCacheKey, TManagerItem}" />.
        /// </summary>
        /// <returns>
        /// An instance of type
        /// <see cref="IMemoryCacheManager{TCacheKey, TManagerItem}" />.
        /// </returns>
        IMemoryCacheManager<TCacheKey, TManagerItem> Create();
    }
}