using System;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Dfe.Spi.Common.Http.Client
{
    /// <summary>
    /// OAuth 2 authenticator using client credentials
    /// </summary>
    public class OAuth2ClientCredentialsAuthenticator : IAuthenticator
    {
        private readonly string _tokenEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _resource;

        /// <summary>
        /// Create an authenticator for OAuth2 client credentials
        /// </summary>
        /// <param name="tokenEndpoint">Full url used to get token</param>
        /// <param name="clientId">Client id to use to authenticate</param>
        /// <param name="clientSecret">Client secret to use to authenticate</param>
        /// <param name="resource">The resource to authenticate for</param>
        public OAuth2ClientCredentialsAuthenticator(
            string tokenEndpoint,
            string clientId,
            string clientSecret,
            string resource)
        {
            _tokenEndpoint = tokenEndpoint;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _resource = resource;
        }

        /// <inheritdoc cref="RestSharp.Authenticators.IAuthenticator"/>
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var token = GetToken();
            request.AddParameter("Authorization", $"Bearer {token}", ParameterType.HttpHeader);
        }

        private string GetToken()
        {
            var client = new RestClient();

            var request = new RestRequest(_tokenEndpoint, Method.POST);
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);
            request.AddParameter("client_id", _clientId, ParameterType.GetOrPost);
            request.AddParameter("client_secret", _clientSecret, ParameterType.GetOrPost);
            request.AddParameter("resource", _resource, ParameterType.GetOrPost);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new OAuth2ClientCredentialsException(response.StatusCode, response.Content);
            }

            var tokenResult = JsonConvert.DeserializeObject<OAuthTokenResult>(response.Content);
            return tokenResult.AccessToken;
        }
        
        private class OAuthTokenResult
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }

    /// <summary>
    /// OAuth 2 Client Credentials exception
    /// </summary>
    public class OAuth2ClientCredentialsException : Exception
    {
        /// <summary>
        /// Create exception based on http response details
        /// </summary>
        /// <param name="statusCode">Status code returned for request</param>
        /// <param name="content">Content returned in response</param>
        public OAuth2ClientCredentialsException(HttpStatusCode statusCode, string content)
            : base($"Failed to get OAuth token. Status {statusCode}, Details: {content}")
        {
        }
    }
}