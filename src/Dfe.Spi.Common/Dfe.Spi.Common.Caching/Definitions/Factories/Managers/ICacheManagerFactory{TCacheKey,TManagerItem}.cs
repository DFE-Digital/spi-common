namespace Dfe.Spi.Common.Caching.Definitions.Factories.Managers
{
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Managers;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="ICacheManager{TCacheKey, TManagerItem} "/> factory.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public interface ICacheManagerFactory<TCacheKey, TManagerItem>
         where TManagerItem : class
    {
        /// <summary>
        /// The method, injected into the created
        /// <see cref="ICacheManager{TCacheKey, TManagerItem}" /> that is
        /// invoked upon initialising a cache item that cannot be found in the
        /// cache.
        /// </summary>
        /// <param name="cacheKey">
        /// The key.
        /// </param>
        /// <returns>
        /// An instance of type <typeparamref name="TCacheKey" />.
        /// </returns>
        Task<TManagerItem> InitialiseCacheItemAsync(TCacheKey cacheKey);
    }
}