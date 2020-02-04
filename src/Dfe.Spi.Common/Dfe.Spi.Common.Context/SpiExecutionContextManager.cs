namespace Dfe.Spi.Common.Context
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Common.Context.Definitions;
    using Dfe.Spi.Common.Context.Models;

    /// <summary>
    /// Implements <see cref="ISpiExecutionContextManager" />.
    /// </summary>
    public abstract class SpiExecutionContextManager
        : ISpiExecutionContextManager
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiExecutionContextManager" /> class.
        /// </summary>
        public SpiExecutionContextManager()
        {
            // Does nothing, for now.
        }

        /// <inheritdoc />
        public abstract SpiExecutionContext SpiExecutionContext
        {
            get;
        }

        /// <inheritdoc />
        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303",
            Justification = "Will not be localised.")]
        public void SetInternalRequestId(Guid internalRequestId)
        {
            if (this.SpiExecutionContext == null)
            {
                throw new InvalidOperationException(
                    $"No context initialised. Make sure to set up " +
                    $"{nameof(this.SpiExecutionContext)} *prior* to calling " +
                    $"this method.");
            }

            this.SpiExecutionContext.InternalRequestId = internalRequestId;
        }
    }
}