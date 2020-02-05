namespace Dfe.Spi.Common.Http.Client
{
    using System;
    using System.Globalization;
    using Dfe.Spi.Common.Context.Models;
    using Microsoft.Net.Http.Headers;
    using RestSharp;

    /// <summary>
    /// Contains extension methods for the <see cref="RestRequest" />
    /// class.
    /// </summary>
    public static class RestRequestExtensions
    {
        private const string BearerAuthorizationFormat = "Bearer {0}";

        /// <summary>
        /// Looks at the input <paramref name="spiExecutionContext" />, and
        /// uses the values to automatically populate the headers of the
        /// <paramref name="restRequest" />.
        /// </summary>
        /// <param name="restRequest">
        /// An instance of <see cref="RestRequest" />.
        /// </param>
        /// <param name="spiExecutionContext">
        /// An instance of <see cref="SpiExecutionContext" />.
        /// </param>
        public static void AppendContext(
            this RestRequest restRequest,
            SpiExecutionContext spiExecutionContext)
        {
            if (restRequest == null)
            {
                throw new ArgumentNullException(nameof(restRequest));
            }

            if (spiExecutionContext == null)
            {
                throw new ArgumentNullException(nameof(spiExecutionContext));
            }

            string identityToken = null;
            if (spiExecutionContext != null)
            {
                identityToken = spiExecutionContext.IdentityToken;
            }

            if (!string.IsNullOrEmpty(identityToken))
            {
                string authorizationValue = string.Format(
                    CultureInfo.InvariantCulture,
                    BearerAuthorizationFormat,
                    identityToken);

                restRequest.AddHeader(
                    HeaderNames.Authorization,
                    authorizationValue);

                // Remember to also add the internal/external request IDs.
                Guid? internalRequestId =
                    spiExecutionContext.InternalRequestId;
                string value = null;
                if (spiExecutionContext.InternalRequestId.HasValue)
                {
                    value = internalRequestId.Value.ToString();

                    restRequest.AddHeader(
                        SpiHeaderNames.InternalRequestIdHeaderName,
                        value);
                }

                if (!string.IsNullOrEmpty(spiExecutionContext.ExternalRequestId))
                {
                    restRequest.AddHeader(
                        SpiHeaderNames.ExternalRequestIdHeaderName,
                        spiExecutionContext.ExternalRequestId);
                }
            }
        }
    }
}