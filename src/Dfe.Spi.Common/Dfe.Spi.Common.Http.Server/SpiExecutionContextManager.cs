namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Linq;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Http.Server.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// Implements <see cref="ISpiExecutionContextManager" />.
    /// </summary>
    public class SpiExecutionContextManager : ISpiExecutionContextManager
    {
        private const string BearerAuthorizationPrefix = "Bearer ";
        private const string InternalRequestIdHeaderName = "X-Internal-Request-Id";
        private const string ExternalRequestIdHeaderName = "X-External-Request-Id";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiExecutionContextManager" /> class.
        /// </summary>
        public SpiExecutionContextManager()
        {
            // Does nothing, for now.
        }

        /// <inheritdoc />
        public SpiExecutionContext SpiExecutionContext
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public void SetContext(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                throw new ArgumentNullException(nameof(headerDictionary));
            }

            Guid? internalRequestId = null;
            if (headerDictionary.ContainsKey(InternalRequestIdHeaderName))
            {
                string internalRequestIdStr =
                    headerDictionary[InternalRequestIdHeaderName].First();

                internalRequestId = Guid.Parse(internalRequestIdStr);
            }

            string externalRequestId = null;
            if (headerDictionary.ContainsKey(ExternalRequestIdHeaderName))
            {
                externalRequestId =
                    headerDictionary[ExternalRequestIdHeaderName].First();
            }

            string identityToken = null;
            if (headerDictionary.ContainsKey(HeaderNames.Authorization))
            {
                string authorizationValue =
                    headerDictionary[HeaderNames.Authorization].First();

                if (authorizationValue.StartsWith(BearerAuthorizationPrefix, StringComparison.InvariantCulture))
                {
                    identityToken = authorizationValue.Replace(
                        BearerAuthorizationPrefix,
                        string.Empty);
                }
            }

            SpiExecutionContext spiExecutionContext = new SpiExecutionContext()
            {
                InternalRequestId = internalRequestId,
                ExternalRequestId = externalRequestId,
                IdentityToken = identityToken,
            };

            this.SpiExecutionContext = spiExecutionContext;
        }

        /// <inheritdoc />
        public void SetInternalRequestId(Guid internalRequestId)
        {
            if (this.SpiExecutionContext == null)
            {
                throw new InvalidOperationException(
                    $"No context initialised. Make sure to call " +
                    $"{nameof(this.SetContext)} *prior* to calling this " +
                    $"method.");
            }

            this.SpiExecutionContext.InternalRequestId = internalRequestId;
        }
    }
}