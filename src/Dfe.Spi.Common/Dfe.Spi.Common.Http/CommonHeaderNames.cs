namespace Dfe.Spi.Common.Http
{
    /// <summary>
    /// Constants class, containing names of commonly used headers used throughout
    /// the SPI.
    /// </summary>
    public static class CommonHeaderNames
    {
        /// <summary>
        /// The EAPIM subscription key header name
        /// </summary>
        public const string EapimSubscriptionKeyHeaderName = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// The Azure Functions authorization key header name
        /// </summary>
        public const string AzureFunctionKeyHeaderName = "X-Functions-Key";
    }
}