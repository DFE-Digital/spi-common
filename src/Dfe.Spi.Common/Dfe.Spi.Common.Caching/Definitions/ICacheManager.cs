namespace Dfe.Spi.Common.Caching.Definitions.Managers
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes the operations of the cache manager.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Gets an item from the manager.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// The cached item, as an <see cref="object" />.
        /// </returns>
        Task<object> GetAsync(
            string key,
            CancellationToken cancellationToken);
    }
}