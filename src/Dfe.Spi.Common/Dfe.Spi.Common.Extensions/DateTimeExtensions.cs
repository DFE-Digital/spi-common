namespace Dfe.Spi.Common.Extensions
{
    using System;
    
    /// <summary>
    /// Contains extension methods for the <see cref="System.DateTime" /> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert to common string format within SPI
        /// </summary>
        /// <param name="value">Date/Time to convert</param>
        /// <returns>String representation of date/time that is common across SPI (ISO format)</returns>
        public static string ToSpiString(this DateTime value)
        {
            return value.ToString("O");
        }
    }
}