namespace Dfe.Spi.Common.Logging.Definitions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes the operations of the logger wrapper.
    /// </summary>
    public interface ILoggerWrapper
    {
        /// <summary>
        /// Logs a <paramref name="message" /> with debug-level importance.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Debug(string message);

        /// <summary>
        /// Logs a <paramref name="message" /> with info-level importance.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Info(string message);

        /// <summary>
        /// Logs a <paramref name="message" /> with warning-level importance.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Warning(string message);

        /// <summary>
        /// Logs a <paramref name="message" /> with warning-level importance.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        /// <param name="exception">
        /// The <see cref="Exception" /> to log.
        /// </param>
        void Warning(string message, Exception exception);

        /// <summary>
        /// Logs a <paramref name="message" /> and an <see cref="Exception" />.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        /// <param name="exception">
        /// The <see cref="Exception" /> to log.
        /// </param>
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1716",
            Justification = "Naming logging functions after the level itself is an accepted standard.")]
        void Error(string message, Exception exception);
    }
}