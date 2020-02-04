namespace Dfe.Spi.Common.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Common.Context.Definitions;
    using Dfe.Spi.Common.Context.Models;
    using Dfe.Spi.Common.Logging.Definitions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements <see cref="ILoggerWrapper" />.
    /// </summary>
    public class LoggerWrapper : ILoggerWrapper
    {
        private static readonly string LogMessagePattern =
            "{message} " +
            $"({nameof(SpiExecutionContext.InternalRequestId)}: {{{nameof(SpiExecutionContext.InternalRequestId)}}}, " +
            $"{nameof(SpiExecutionContext.ExternalRequestId)}: {{{nameof(SpiExecutionContext.ExternalRequestId)}}})";

        private readonly ILogger logger;
        private readonly ISpiExecutionContextManager spiExecutionContextManager;

        private readonly Action<ILogger, string, Guid?, string, Exception> logDebug;
        private readonly Action<ILogger, string, Guid?, string, Exception> logInfo;
        private readonly Action<ILogger, string, Guid?, string, Exception> logWarning;
        private readonly Action<ILogger, string, Guid?, string, Exception> logError;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerWrapper"/> class.
        /// class.
        /// </summary>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        /// <param name="spiExecutionContextManager">
        /// An instance of type <see cref="ISpiExecutionContextManager" />.
        /// </param>
        public LoggerWrapper(
            ILogger logger,
            ISpiExecutionContextManager spiExecutionContextManager)
        {
            this.logger = logger;
            this.spiExecutionContextManager = spiExecutionContextManager;

            this.logDebug = LoggerMessage.Define<string, Guid?, string>(
                LogLevel.Debug,
                new EventId(1),
                LogMessagePattern);
            this.logInfo = LoggerMessage.Define<string, Guid?, string>(
                LogLevel.Information,
                new EventId(2),
                LogMessagePattern);
            this.logWarning = LoggerMessage.Define<string, Guid?, string>(
                LogLevel.Warning,
                new EventId(3),
                LogMessagePattern);
            this.logError = LoggerMessage.Define<string, Guid?, string>(
                LogLevel.Error,
                new EventId(4),
                LogMessagePattern);
        }

        /// <inheritdoc />
        public void Debug(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logDebug(
                this.logger,
                message,
                this.spiExecutionContextManager.SpiExecutionContext.InternalRequestId,
                this.spiExecutionContextManager.SpiExecutionContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logError(
                this.logger,
                message,
                this.spiExecutionContextManager.SpiExecutionContext.InternalRequestId,
                this.spiExecutionContextManager.SpiExecutionContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Info(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logInfo(
                this.logger,
                message,
                this.spiExecutionContextManager.SpiExecutionContext.InternalRequestId,
                this.spiExecutionContextManager.SpiExecutionContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logWarning(
                this.logger,
                message,
                this.spiExecutionContextManager.SpiExecutionContext.InternalRequestId,
                this.spiExecutionContextManager.SpiExecutionContext.ExternalRequestId,
                exception);
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303",
            Justification = "Library will not be localised - a resources file for this is overkill.")]
        private void AssertContextSet()
        {
            if (this.spiExecutionContextManager == null || this.spiExecutionContextManager.SpiExecutionContext == null)
            {
                throw new InvalidOperationException(
                    $"Either no {nameof(ISpiExecutionContextManager)} " +
                    $"instance was injected, or no " +
                    $"{nameof(ISpiExecutionContextManager)}.{nameof(this.spiExecutionContextManager.SpiExecutionContext)} " +
                    $"has been set. The " +
                    $"{nameof(this.spiExecutionContextManager.SpiExecutionContext)} " +
                    $"must be set prior to calling this method.");
            }
        }
    }
}