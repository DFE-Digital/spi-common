﻿namespace Dfe.Spi.Common.Http.Client
{
    using System;
    using System.Globalization;
    using System.Net;
    using Dfe.Spi.Common.Models;

    /// <summary>
    /// Represents an error raised upon calling a SPI web service, and it
    /// returning a non-successful status code, and optionally, a
    /// <see cref="HttpErrorBody" /> instance.
    /// </summary>
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
        public SpiWebServiceException()
        {
            // Nothing - used for serialisation.
        }

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiWebServiceException" /> class.
        /// </summary>
        /// <param name="message">
        /// The text of the error message.
        /// </param>
        public SpiWebServiceException(string message)
            : base(message)
        {
            // Used if we're inheriting from this class.
        }

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiWebServiceException" /> class.
        /// </summary>
        /// <param name="message">
        /// The text of the error message.
        /// </param>
        /// <param name="innerException">
        /// A nested exception.
        /// </param>
        public SpiWebServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
            // Used if we're inheriting from this class.
        }

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SpiWebServiceException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">
        /// The actual <see cref="HttpStatusCode" /> resulting in the exception
        /// being thrown.
        /// </param>
        /// <param name="httpErrorBody">
        /// An instance of type <see cref="HttpErrorBody" />.
        /// </param>
        public SpiWebServiceException(
            HttpStatusCode httpStatusCode,
            HttpErrorBody httpErrorBody)
            : base(BuildExceptionMessage(httpStatusCode))
        {
            this.HttpStatusCode = httpStatusCode;
            this.HttpErrorBody = httpErrorBody;
        }

        /// <summary>
        /// Gets or sets the actual <see cref="HttpStatusCode" /> resulting in
        /// the exception being thrown.
        /// </summary>
        public HttpStatusCode HttpStatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Models.HttpErrorBody" />, containing
        /// detail regarding the call to the web service.
        /// </summary>
        public HttpErrorBody HttpErrorBody
        {
            get;
            set;
        }

        private static string BuildExceptionMessage(
            HttpStatusCode httpStatusCode)
        {
            string toReturn = null;

            int statusCode = (int)httpStatusCode;

            toReturn = string.Format(
                CultureInfo.InvariantCulture,
                Message,
                statusCode);

            return toReturn;
        }
    }
}
