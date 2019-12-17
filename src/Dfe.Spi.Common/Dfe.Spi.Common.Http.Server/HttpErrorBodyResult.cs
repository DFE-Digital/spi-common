namespace Dfe.Spi.Common.Http.Server
{
    using System.Net;
    using Dfe.Spi.Common.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// An action result which returns error information.
    /// </summary>
    public class HttpErrorBodyResult : JsonResult
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="HttpErrorBodyResult" /> class.
        /// </summary>
        /// <param name="httpStatusCode">
        /// The status code. Inherited from the Azure API management error
        /// model.
        /// </param>
        /// <param name="errorIdentifier">
        /// An error identifier. The format of this identifier is dependent on
        /// the local system.
        /// </param>
        /// <param name="message">
        /// An error message. Inherited from the Azure API management error
        /// model.
        /// </param>
        public HttpErrorBodyResult(
            HttpStatusCode httpStatusCode,
            string errorIdentifier,
            string message)
            : base(
                CreateHttpErrorBody(httpStatusCode, errorIdentifier, message))
        {
            // Nothing - just bubbles down.
        }

        private static HttpErrorBody CreateHttpErrorBody(
            HttpStatusCode httpStatusCode,
            string errorIdentifier,
            string message)
        {
            HttpErrorBody toReturn = new HttpErrorBody()
            {
                StatusCode = httpStatusCode,
                ErrorIdentifier = errorIdentifier,
                Message = message,
            };

            return toReturn;
        }
    }
}