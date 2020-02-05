namespace Dfe.Spi.Common.Http
{
    /// <summary>
    /// Constants class, containing names of custom headers used throughout
    /// the SPI.
    /// </summary>
    public static class SpiHeaderNames
    {
        /// <summary>
        /// The internal request ID header name.
        /// </summary>
        public const string InternalRequestIdHeaderName = "X-Internal-Request-Id";

        /// <summary>
        /// The external request ID header name.
        /// </summary>
        public const string ExternalRequestIdHeaderName = "X-External-Request-Id";
    }
}