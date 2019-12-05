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
        /// Initializes a new instance of the <see cref="RequestContext"/> class.
        /// class.
        /// </summary>
        internal RequestContext()
        {
            // Nothing - just prevents this being created outside of the
            // assembly.
        }

        /// <summary>
        /// Gets or sets an internal request id.
        /// </summary>
        public Guid? InternalRequestId
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