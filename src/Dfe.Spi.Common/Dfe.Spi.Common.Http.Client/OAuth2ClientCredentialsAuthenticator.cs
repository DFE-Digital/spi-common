namespace Dfe.Spi.Common.Http.Client
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// OAuth 2 authenticator using client credentials.
    /// </summary>
    public class OAuth2ClientCredentialsAuthenticator : IAuthenticator
    {
        private readonly string tokenEndpoint;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string resource;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="OAuth2ClientCredentialsAuthenticator" /> class.
        /// </summary>
        /// <param name="tokenEndpoint">
        /// Full url used to get token.
        /// </param>
        /// <param name="clientId">
        /// Client id to use to authenticate.
        /// </param>
        /// <param name="clientSecret">
        /// Client secret to use to authenticate.
        /// </param>
        /// <param name="resource">
        /// The resource to authenticate for.
        /// </param>
        public OAuth2ClientCredentialsAuthenticator(
            string tokenEndpoint,
            string clientId,
            string clientSecret,
            string resource)
        {
            this.tokenEndpoint = tokenEndpoint;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.resource = resource;
        }

        /// <inheritdoc cref="RestSharp.Authenticators.IAuthenticator"/>
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var token = this.GetToken();

            request.AddParameter(
                "Authorization",
                $"Bearer {token}",
                ParameterType.HttpHeader);
        }

        private string GetToken()
        {
            var client = new RestClient();

            var request = new RestRequest(this.tokenEndpoint, Method.POST);

            request.AddParameter(
                "grant_type",
                "client_credentials",
                ParameterType.GetOrPost);

            request.AddParameter(
                "client_id",
                this.clientId,
                ParameterType.GetOrPost);

            request.AddParameter(
                "client_secret",
                this.clientSecret,
                ParameterType.GetOrPost);

            request.AddParameter(
                "resource",
                this.resource,
                ParameterType.GetOrPost);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new OAuth2ClientCredentialsException(
                    response.StatusCode,
                    response.Content);
            }

            var tokenResult = JsonConvert.DeserializeObject<OAuthTokenResult>(
                response.Content);

            return tokenResult.AccessToken;
        }

        /// <summary>
        /// Represents the JSON content returned by an OAuth token request.
        /// </summary>
        private class OAuthTokenResult
        {
            /// <summary>
            /// Gets or sets the <c>access_token</c>.
            /// </summary>
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }
}