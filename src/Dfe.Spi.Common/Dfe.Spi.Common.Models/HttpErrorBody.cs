namespace Dfe.Spi.Common.Models
{
    using System.Net;

    /// <summary>
    /// Common HTTP error body model, to be returned with all non-successful
    /// status codes. Extends the Azure API management error model.
    /// </summary>
    public class HttpErrorBody : ModelsBase
    {
        /// <summary>
        /// Gets or sets an error identifier. The format of this identifier
        /// is dependent on the local system.
        /// </summary>
        public string ErrorIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an error message.
        /// Inherited from the Azure API management error model.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status code.
        /// Inherited from the Azure API management error model.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get;
            set;
        }
    }
}