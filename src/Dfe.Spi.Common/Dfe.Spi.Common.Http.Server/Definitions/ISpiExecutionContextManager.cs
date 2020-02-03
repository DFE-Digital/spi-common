namespace Dfe.Spi.Common.Http.Server.Definitions
{
    using System;
    using Dfe.Spi.Common.Http.Server.Models;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Describes the operations of the
    /// <see cref="Models.SpiExecutionContext" /> manager.
    /// </summary>
    public interface ISpiExecutionContextManager
    {
        /// <summary>
        /// Gets the current <see cref="Models.SpiExecutionContext" />.
        /// </summary>
        SpiExecutionContext SpiExecutionContext
        {
            get;
        }

        /// <summary>
        /// Sets the <see cref="Models.SpiExecutionContext" />, based on
        /// information pulled from the <see cref="HttpRequest" />.
        /// </summary>
        /// <param name="headerDictionary">
        /// An instance of type <see cref="IHeaderDictionary" />.
        /// </param>
        void SetContext(IHeaderDictionary headerDictionary);

        /// <summary>
        /// Sets the internal request id (i.e. custom dimension information)
        /// against the instance.
        /// </summary>
        /// <param name="internalRequestId">
        /// Guid to use as internal request id.
        /// </param>
        void SetInternalRequestId(Guid internalRequestId);
    }
}