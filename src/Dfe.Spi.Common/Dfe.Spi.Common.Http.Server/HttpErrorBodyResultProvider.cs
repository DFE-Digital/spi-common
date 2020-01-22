namespace Dfe.Spi.Common.Http.Server
{
    using System.Globalization;
    using System.Net;
    using System.Resources;
    using Dfe.Spi.Common.Http.Server.Definitions;

    /// <summary>
    /// Implements <see cref="IHttpErrorBodyResultProvider" />.
    /// </summary>
    public class HttpErrorBodyResultProvider : IHttpErrorBodyResultProvider
    {
        private const string ErrorIdentifierFormat = "SPI-{0}-{1}";
        private const string ResourceIdentifierFormat = "_{0}";

        private readonly string systemErrorIdentifier;
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="HttpErrorBodyResultProvider" /> class.
        /// </summary>
        /// <param name="systemErrorIdentifier">
        /// A host system identifier to use error code. For example, "ESQ"
        /// (Entity SQuasher) or "T" (Translation).
        /// </param>
        /// <param name="resourceManager">
        /// An instance of the host system's <see cref="ResourceManager" />.
        /// </param>
        public HttpErrorBodyResultProvider(
            string systemErrorIdentifier,
            ResourceManager resourceManager)
        {
            this.systemErrorIdentifier = systemErrorIdentifier;
            this.resourceManager = resourceManager;
        }

        /// <inheritdoc />
        public HttpErrorBodyResult GetHttpErrorBodyResult(
            HttpStatusCode httpStatusCode,
            int errorIdentifierInt,
            params string[] messageArguments)
        {
            HttpErrorBodyResult toReturn = null;

            string errorIdentifier = string.Format(
                CultureInfo.InvariantCulture,
                ErrorIdentifierFormat,
                this.systemErrorIdentifier,
                errorIdentifierInt);

            string name = string.Format(
                CultureInfo.InvariantCulture,
                ResourceIdentifierFormat,
                errorIdentifierInt);

            string message = this.resourceManager.GetString(
                name,
                CultureInfo.InvariantCulture);

            message = string.Format(
                CultureInfo.InvariantCulture,
                message,
                messageArguments);

            toReturn = new HttpErrorBodyResult(
                httpStatusCode,
                errorIdentifier,
                message);

            return toReturn;
        }
    }
}