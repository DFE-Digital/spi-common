using System;
using NodaTime;
using NodaTime.Text;

namespace Dfe.Spi.Common.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains extension methods for the <see cref="string" /> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string from PascalCase to kebab-case.
        /// Method based on answer from
        /// <see href="https://stackoverflow.com/questions/37301287/how-do-i-convert-pascalcase-to-kebab-case-with-c" />.
        /// </summary>
        /// <param name="value">
        /// The string in PascalFormat, to convert to kebab-case.
        /// </param>
        /// <returns>
        /// The original <paramref name="value" />, but in kebab-case.
        /// </returns>
        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1308",
            Justification = "I need lowercase for kebab-case, not uppercase.")]
        public static string PascalToKebabCase(this string value)
        {
            string toReturn = Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower(CultureInfo.InvariantCulture);

            return toReturn;
        }


        /// <summary>
        /// Parse the string to a Date/Time using the valid SPI formats
        /// </summary>
        /// <param name="value">string representation of date/time</param>
        /// <returns>Date/Time</returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ToDateTime(this string value)
        {
            var isoMatch = Regex.Match(value, @"((\+[0-9]{2}:[0-9]{2})|Z)$");
            if (isoMatch.Success)
            {
                var offset = isoMatch.Groups[1].Value;
                var dateTimeString = value.Substring(0, value.Length - offset.Length);
                var parsedDateTime = IsoDateTimePattern.Parse(dateTimeString);
                if (!parsedDateTime.Success)
                {
                    throw new InvalidDateTimeFormatException($"Unable to parse {value} to DateTime", SupportedDateTimePatterns, parsedDateTime.Exception);
                }

                var parsedOffset = OffsetPattern.GeneralInvariantWithZ.Parse(offset);
                if (!parsedOffset.Success)
                {
                    throw new InvalidDateTimeFormatException($"Unable to parse {value} to DateTime", SupportedDateTimePatterns, parsedOffset.Exception);
                }
                
                var offsetDateTime = new OffsetDateTime(parsedDateTime.Value, parsedOffset.Value);
                return offsetDateTime.ToDateTimeOffset().UtcDateTime;
            }
            

            Exception lastException = null;
            foreach (var localDatePattern in SupportedDateTimePatterns)
            {
                try
                {
                    var parsed = localDatePattern.Parse(value);
                    if (parsed.Success)
                    {
                        return parsed.Value.ToDateTimeUnspecified();
                    }

                    lastException = parsed.Exception;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            throw new InvalidDateTimeFormatException($"Unable to parse {value} to DateTime", 
                SupportedDateTimePatterns, lastException);
        }
        
        private static readonly CultureInfo DateTimeParsingCulture = new CultureInfo("en-GB");

        private static readonly LocalDateTimePattern IsoDateTimePattern =
            LocalDateTimePattern.Create("yyyy-MM-ddTHH:mm:ss", DateTimeParsingCulture);
        private static readonly LocalDateTimePattern[] SupportedDateTimePatterns = new[]
        {
            LocalDateTimePattern.Create("yyyy-MM-dd", DateTimeParsingCulture),
            LocalDateTimePattern.Create("yyyy-MM-dd HH:mm:ss", DateTimeParsingCulture),
            LocalDateTimePattern.Create("yyyy/MM/dd", DateTimeParsingCulture),
            LocalDateTimePattern.Create("yyyy/MM/dd HH:mm:ss", DateTimeParsingCulture),
            LocalDateTimePattern.Create("yyyy.MM.dd", DateTimeParsingCulture),
            LocalDateTimePattern.Create("yyyy.MM.dd HH:mm:ss", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd-MM-yyyy", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd-MM-yyyy HH:mm:ss", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd/MM/yyyy", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd/MM/yyyy HH:mm:ss", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd.MM.yyyy", DateTimeParsingCulture),
            LocalDateTimePattern.Create("dd.MM.yyyy HH:mm:ss", DateTimeParsingCulture),
            IsoDateTimePattern,
        };

    }
}