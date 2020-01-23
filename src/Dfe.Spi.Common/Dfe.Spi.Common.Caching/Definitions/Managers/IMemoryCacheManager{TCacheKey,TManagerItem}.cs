namespace Dfe.Spi.Common.Caching.Definitions.Managers
{
    /// <summary>
    /// Describes the operations of the memory cache manager.
    /// </summary>
    /// <typeparam name="TCacheKey">
    /// The type of key used in the underlying storage.
    /// </typeparam>
    /// <typeparam name="TManagerItem">
    /// The type of item being managed.
    /// </typeparam>
    public interface IMemoryCacheManager<TCacheKey, TManagerItem>
        : ICacheManager<TCacheKey, TManagerItem>
        where TManagerItem : class
    {
        // Nothing for now.
    }
}