namespace Dfe.Spi.Common.Context.Definitions
{
    using System;
    using Dfe.Spi.Common.Context.Models;

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
        /// Sets the internal request id (i.e. custom dimension information)
        /// against the instance.
        /// </summary>
        /// <param name="internalRequestId">
        /// Guid to use as internal request id.
        /// </param>
        void SetInternalRequestId(Guid internalRequestId);
    }
}
