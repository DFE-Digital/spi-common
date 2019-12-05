namespace Dfe.Spi.Common.Logging.Models
{
    using System;

    /// <summary>
    /// Contains extra context around an initial request, to store against
    /// a <see cref="LoggerWrapper" />.
    /// </summary>
    public class RequestContext
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