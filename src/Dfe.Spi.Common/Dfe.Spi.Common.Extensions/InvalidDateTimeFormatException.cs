using System;
using System.Linq;
using NodaTime.Text;

namespace Dfe.Spi.Common.Extensions
{
    /// <summary>
    /// Exception describing failure to parse date/time
    /// </summary>
    public class InvalidDateTimeFormatException : Exception
    {
        /// <summary>
        /// Create new InvalidDateTimeFormatException
        /// </summary>
        /// <param name="message">Description of error</param>
        /// <param name="supportedPatterns">Supported formats</param>
        /// <param name="innerException">Related exception</param>
        public InvalidDateTimeFormatException(string message, LocalDateTimePattern[] supportedPatterns, Exception innerException = null)
            : base (BuildMessage(message, supportedPatterns.Select(p=>p.PatternText).ToArray()), innerException)
        {
        }

        private static string BuildMessage(string message, string[] supportedFormats)
        {
            var supportedFormatsForMessage = supportedFormats.Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
            return $"{message}{Environment.NewLine}Supported formats:{Environment.NewLine}{supportedFormatsForMessage}";
        }
    }
}