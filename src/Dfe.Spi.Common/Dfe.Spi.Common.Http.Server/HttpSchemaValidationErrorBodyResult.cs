using System.Linq;
using System.Net;
using Dfe.Spi.Common.Models;
using NJsonSchema.Validation;

namespace Dfe.Spi.Common.Http.Server
{
    /// <summary>
    /// Error result when json schema validation fails
    /// </summary>
    public class HttpSchemaValidationErrorBodyResult : HttpErrorBodyResult
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HttpSchemaValidationErrorBodyResult" /> class.
        /// </summary>
        /// <param name="httpStatusCode">
        /// The status code. Inherited from the Azure API management error
        /// model.
        /// </param>
        /// <param name="errorIdentifier">
        /// An error identifier. The format of this identifier is dependent on
        /// the local system.
        /// </param>
        /// <param name="validationException">Schema validation exception</param>
        public HttpSchemaValidationErrorBodyResult(
            HttpStatusCode httpStatusCode,
            string errorIdentifier,
            JsonSchemaValidationException validationException)
            : base(BuildErrorBody(httpStatusCode, errorIdentifier, validationException))
        {
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="HttpSchemaValidationErrorBodyResult" /> class.
        /// </summary>
        /// <param name="errorIdentifier">
        /// An error identifier. The format of this identifier is dependent on
        /// the local system.
        /// </param>
        /// <param name="validationException">Schema validation exception</param>
        public HttpSchemaValidationErrorBodyResult(
            string errorIdentifier,
            JsonSchemaValidationException validationException)
            : this(HttpStatusCode.BadRequest, errorIdentifier, validationException)
        {
        }

        private static HttpDetailedErrorBody BuildErrorBody(
            HttpStatusCode httpStatusCode,
            string errorIdentifier,
            JsonSchemaValidationException validationException)
        {
            return new HttpDetailedErrorBody
            {
                StatusCode = httpStatusCode,
                ErrorIdentifier = errorIdentifier,
                Message = validationException.Message,
                Details = validationException.ValidationErrors.Select(GetValidationErrorDetailsString).ToArray(),
            };
        }

        private static string GetValidationErrorDetailsString(ValidationError error)
        {
            if (error.Kind == ValidationErrorKind.NotInEnumeration)
            {
                var validEnumValues = error.Schema.Enumeration
                    .Select(x => x.ToString())
                    .Aggregate((x, y) => $"{x}, {y}");
                return $"{error.Path}: {error.Kind}. Valid values are {validEnumValues}";
            }
            
            return $"{error.Path}: {error.Kind}";
        }
    }
}