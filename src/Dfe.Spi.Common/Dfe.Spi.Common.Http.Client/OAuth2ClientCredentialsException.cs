namespace Dfe.Spi.Common.Http.Client
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;

    /// <summary>
    /// OAuth 2 Client Credentials exception.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "Not a public library.")]
    public class OAuth2ClientCredentialsException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="OAuth2ClientCredentialsException" /> class.
        /// </summary>
        /// <param name="statusCode">
        /// Status code returned for request.
        /// </param>
        /// <param name="content">
        /// Content returned in response.
        /// </param>
        public OAuth2ClientCredentialsException(
            HttpStatusCode statusCode,
            string content)
            : base($"Failed to get OAuth token. Status {statusCode}, Details: {content}")
        {
            // Nothing.
        }
    }
}