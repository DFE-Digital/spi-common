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
        public virtual void SetContext(IHeaderDictionary headerDictionary)
        {
            var spiExecutionContext = new SpiExecutionContext();
            ReadHeadersIntoContext(headerDictionary, spiExecutionContext);

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

        /// <summary>
        /// Set properties of context from HTTP headers.
        /// </summary>
        /// <param name="headerDictionary">HTTP headers to read from.</param>
        /// <param name="context">Context to read headers into.</param>
        /// <exception cref="ArgumentNullException">Thrown if headers or context is null</exception>
        protected virtual void ReadHeadersIntoContext(IHeaderDictionary headerDictionary, SpiExecutionContext context)
        {
            if (headerDictionary == null)
            {
                throw new ArgumentNullException(nameof(headerDictionary));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (headerDictionary.ContainsKey(SpiHeaderNames.InternalRequestIdHeaderName))
            {
                string internalRequestIdStr =
                    headerDictionary[SpiHeaderNames.InternalRequestIdHeaderName].First();

                context.InternalRequestId = Guid.Parse(internalRequestIdStr);
            }

            if (headerDictionary.ContainsKey(SpiHeaderNames.ExternalRequestIdHeaderName))
            {
                context.ExternalRequestId =
                    headerDictionary[SpiHeaderNames.ExternalRequestIdHeaderName].First();
            }

            if (headerDictionary.ContainsKey(HeaderNames.Authorization))
            {
                string authorizationValue =
                    headerDictionary[HeaderNames.Authorization].First();

                string authorizationValueUpper =
                    authorizationValue.ToUpperInvariant();

                if (authorizationValueUpper.StartsWith(BearerAuthorizationPrefix, StringComparison.InvariantCulture))
                {
                    context.IdentityToken = authorizationValue.Substring(
                        BearerAuthorizationPrefix.Length,
                        authorizationValue.Length - BearerAuthorizationPrefix.Length);
                }
            }
        }
    }
}