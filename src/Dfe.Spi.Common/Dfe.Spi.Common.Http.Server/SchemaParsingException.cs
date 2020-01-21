namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Newtonsoft.Json;
    using NJsonSchema;

    /// <summary>
    /// Represents an error raised upon parsing a corrupt/not well formed
    /// embedded <see cref="JsonSchema" />.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "This is internally used, and will not be serialised.")]
    [ExcludeFromCodeCoverage]
    public class SchemaParsingException : Exception
    {
        private new const string Message =
            "The embedded schema file, \"{0}\", exists, but is either " +
            "corrupt or not well-formed. Inspect the inner exception for " +
            "more detail.";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="SchemaParsingException" /> class.
        /// </summary>
        /// <param name="fullName">
        /// The full name to the embedded schema.
        /// </param>
        /// <param name="jsonReaderException">
        /// An instance of <see cref="JsonReaderException" />.
        /// </param>
        public SchemaParsingException(
            string fullName,
            JsonReaderException jsonReaderException)
            : base(
                  BuildExceptionMessage(fullName),
                  jsonReaderException)
        {
            // Nothing for now...
        }

        private static string BuildExceptionMessage(string fullName)
        {
            string toReturn = string.Format(
                CultureInfo.InvariantCulture,
                Message,
                fullName);

            return toReturn;
        }
    }
}