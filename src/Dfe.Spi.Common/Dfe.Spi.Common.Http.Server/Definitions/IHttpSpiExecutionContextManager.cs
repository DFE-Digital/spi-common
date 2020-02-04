namespace Dfe.Spi.Common.Http.Server.Definitions
{
    using Dfe.Spi.Common.Context.Definitions;
    using Dfe.Spi.Common.Context.Models;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="IHttpSpiExecutionContextManager" />.
    /// </summary>
    public interface IHttpSpiExecutionContextManager
        : ISpiExecutionContextManager
    {
        /// <summary>
        /// Sets the underling <see cref="SpiExecutionContext" /> after reading
        /// the headers present in the <paramref name="headerDictionary" />.
        /// </summary>
        /// <param name="headerDictionary">
        /// An instance of type <see cref="IHeaderDictionary" />.
        /// </param>
        void SetContext(IHeaderDictionary headerDictionary);
    }
}