namespace Dfe.Spi.Common.Http.Client
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using Dfe.Spi.Common.Models;

    /// <summary>
    /// Represents an error raised upon calling a SPI web service, and it
    /// returning a non-successful status code, and optionally, a
    /// <see cref="HttpErrorBody" /> instance.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "This is internally used, and will not be serialised.")]
    public class SpiWebServiceException : WebException
    {
        private new const string Message =
            "The SPI web service returned a non-successful status code, " +
            "{0}. See the HttpErrorBody property for more information (if " +
            "available).";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiWebServiceException" /> class.
        /// </summary>
        /// <param name="httpErrorBody">
        /// An instance of type <see cref="HttpErrorBody" />.
        /// </param>
        public SpiWebServiceException(HttpErrorBody httpErrorBody)
            : base(BuildExceptionMessage(httpErrorBody))
        {
            // Nothing - just bubbles down.
        }

        /// <summary>
        /// Gets the <see cref="Models.HttpErrorBody" />, containing detail
        /// regarding the call to the web service.
        /// </summary>
        public HttpErrorBody HttpErrorBody
        {
            get;
            private set;
        }

        private static string BuildExceptionMessage(
            HttpErrorBody httpErrorBody)
        {
            string toReturn = null;

            int statusCode = 0;
            if (httpErrorBody != null)
            {
                statusCode = (int)httpErrorBody.StatusCode;
            }

            toReturn = string.Format(
                CultureInfo.InvariantCulture,
                Message,
                statusCode);

            return toReturn;
        }
    }
}
