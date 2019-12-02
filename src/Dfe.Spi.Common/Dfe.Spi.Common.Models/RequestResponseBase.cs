namespace Dfe.Spi.Common.Models
{
    using System;

    /// <summary>
    /// Abstract base class for all requests and responses.
    /// </summary>
    public abstract class RequestResponseBase
    {
        /// <summary>
        /// Gets or sets an internal request id.
        /// </summary>
        public Guid InternalRequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an external request id.
        /// </summary>
        public string ExternalRequestId
        {
            get;
            set;
        }
    }
}