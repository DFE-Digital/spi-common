namespace Dfe.Spi.Common.Logging
{
    using System;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Common.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements <see cref="ILoggerWrapper" />.
    /// </summary>
    public class LoggerWrapper : ILoggerWrapper
    {
        private readonly ILogger logger;

        private RequestResponseBase requestResponseBase;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerWrapper" />
        /// class.
        /// </summary>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        public LoggerWrapper(
            ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public void SetContext(RequestResponseBase requestResponseBase)
        {
            this.requestResponseBase = requestResponseBase;
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            this.LogMessage(LogLevel.Debug, message);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
            this.LogMessage(LogLevel.Error, message, exception);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.LogMessage(LogLevel.Information, message);
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            this.LogMessage(LogLevel.Warning, message);
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception)
        {
            this.LogMessage(LogLevel.Warning, message, exception);
        }

        private void LogMessage(
            LogLevel logLevel,
            string message,
            Exception exception = null)
        {
            message =
                $"{message} " +
                $"({nameof(RequestResponseBase.InternalRequestId)}: {{{nameof(RequestResponseBase.InternalRequestId)}}}, " +
                $"{nameof(RequestResponseBase.ExternalRequestId)}: {{{nameof(RequestResponseBase.ExternalRequestId)}}})";

            this.logger.Log(
                logLevel,
                exception,
                message,
                this.requestResponseBase.InternalRequestId,
                this.requestResponseBase.ExternalRequestId);
        }
    }
}