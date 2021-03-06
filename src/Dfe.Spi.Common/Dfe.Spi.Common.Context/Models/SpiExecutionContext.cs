﻿namespace Dfe.Spi.Common.Context.Models
{
    using System;

    /// <summary>
    /// Represents exeuction context across the various SPI APIs.
    /// </summary>
    public class SpiExecutionContext : ModelsBase
    {
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

        /// <summary>
        /// Gets or sets an identity token.
        /// </summary>
        public string IdentityToken
        {
            get;
            set;
        }
    }
}