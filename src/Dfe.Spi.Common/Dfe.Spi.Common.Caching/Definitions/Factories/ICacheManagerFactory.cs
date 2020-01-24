namespace Dfe.Spi.Common.Caching.Definitions.Factories.Managers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Caching.Definitions.Managers;

    /// <summary>
    /// Describes the operations of the <see cref="ICacheManager "/> factory.
    /// </summary>
    public interface ICacheManagerFactory
    {
        /// <summary>
        /// Creates an instance of type <see cref="ICacheManager" />.
        /// </summary>
        /// <returns>
        /// An instance of type <see cref="ICacheManager" />.
        /// </returns>
        ICacheManager Create();

        /// <summary>
        /// The method, injected into the created <see cref="ICacheManager" />
        /// that is invoked upon initialising a cache item that cannot be found
        /// in the cache.
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
        Task<object> InitialiseCacheItemAsync(
            string key,
            CancellationToken cancellationToken);
    }
}