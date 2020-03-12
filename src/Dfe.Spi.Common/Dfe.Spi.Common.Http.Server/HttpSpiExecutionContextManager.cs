namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Linq;
    using Dfe.Spi.Common.Context;
    using Dfe.Spi.Common.Context.Models;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// Implements <see cref="IHttpSpiExecutionContextManager" />.
    /// </summary>
    public class HttpSpiExecutionContextManager
        : SpiExecutionContextManager, IHttpSpiExecutionContextManager
    {
        private const string BearerAuthorizationPrefix = "BEARER ";

        private SpiExecutionContext spiExecutionContext;

        /// <inheritdoc />
        public override SpiExecutionContext SpiExecutionContext
        {
            get
            {
                return this.spiExecutionContext;
            }
        }

        /// <inheritdoc />
        public void SetContext(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                throw new ArgumentNullException(nameof(headerDictionary));
            }

            Guid? internalRequestId = null;
            if (headerDictionary.ContainsKey(SpiHeaderNames.InternalRequestIdHeaderName))
            {
                string internalRequestIdStr =
                    headerDictionary[SpiHeaderNames.InternalRequestIdHeaderName].First();

                internalRequestId = Guid.Parse(internalRequestIdStr);
            }

            string externalRequestId = null;
            if (headerDictionary.ContainsKey(SpiHeaderNames.ExternalRequestIdHeaderName))
            {
                externalRequestId =
                    headerDictionary[SpiHeaderNames.ExternalRequestIdHeaderName].First();
            }

            string identityToken = null;
            if (headerDictionary.ContainsKey(HeaderNames.Authorization))
            {
                string authorizationValue =
                    headerDictionary[HeaderNames.Authorization].First();

                string authorizationValueUpper =
                    authorizationValue.ToUpperInvariant();

                if (authorizationValueUpper.StartsWith(BearerAuthorizationPrefix, StringComparison.InvariantCulture))
                {
                    identityToken = authorizationValue.Substring(
                        BearerAuthorizationPrefix.Length,
                        authorizationValue.Length - BearerAuthorizationPrefix.Length);
                }
            }

            SpiExecutionContext spiExecutionContext = new SpiExecutionContext()
            {
                InternalRequestId = internalRequestId,
                ExternalRequestId = externalRequestId,
                IdentityToken = identityToken,
            };

            this.spiExecutionContext = spiExecutionContext;
        }

        /// <inheritdoc />
        public override void SetInternalRequestId(Guid internalRequestId)
        {
            if (this.spiExecutionContext == null)
            {
                this.spiExecutionContext = new SpiExecutionContext();
            }

            base.SetInternalRequestId(internalRequestId);
        }
    }
}