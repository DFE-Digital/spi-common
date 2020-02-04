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
        private const string BearerAuthorizationPrefix = "Bearer ";
        private const string InternalRequestIdHeaderName = "X-Internal-Request-Id";
        private const string ExternalRequestIdHeaderName = "X-External-Request-Id";

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

            this.spiExecutionContext = spiExecutionContext;
        }
    }
}