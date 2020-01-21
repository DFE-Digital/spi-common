namespace Dfe.Spi.Common.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Common.Logging.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements <see cref="ILoggerWrapper" />.
    /// </summary>
    public class LoggerWrapper : ILoggerWrapper
    {
        private const string InternalRequestIdHeaderName = "X-Internal-Request-Id";
        private const string ExternalRequestIdHeaderName = "X-External-Request-Id";
        private static readonly string LogMessagePattern =
            "{message} " +
            $"({nameof(RequestContext.InternalRequestId)}: {{{nameof(RequestContext.InternalRequestId)}}}, " +
            $"{nameof(RequestContext.ExternalRequestId)}: {{{nameof(RequestContext.ExternalRequestId)}}})";

        private readonly ILogger logger;

        private readonly Action<ILogger, string, Guid?, string, Exception> logDebug;
        private readonly Action<ILogger, string, Guid?, string, Exception> logInfo;
        private readonly Action<ILogger, string, Guid?, string, Exception> logWarning;
        private readonly Action<ILogger, string, Guid?, string, Exception> logError;

        private RequestContext requestContext;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerWrapper"/> class.
        /// class.
        /// </summary>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        public LoggerWrapper(ILogger logger)
        {
            this.logger = logger;

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
        public void SetContext(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                throw new ArgumentNullException(nameof(headerDictionary));
            }

            var internalRequestId = headerDictionary.ContainsKey(InternalRequestIdHeaderName)
                ? (Guid?)Guid.Parse(headerDictionary[InternalRequestIdHeaderName].First())
                : null;

            var externalRequestId = headerDictionary.ContainsKey(ExternalRequestIdHeaderName)
                ? headerDictionary[ExternalRequestIdHeaderName].First()
                : null;

            RequestContext requestContext = new RequestContext()
            {
                InternalRequestId = internalRequestId,
                ExternalRequestId = externalRequestId,
            };

            this.requestContext = requestContext;
        }

        /// <inheritdoc />
        public void SetInternalRequestId(Guid internalRequestId)
        {
            if (this.requestContext == null)
            {
                this.requestContext = new RequestContext();
            }

            this.requestContext.InternalRequestId = internalRequestId;
        }

        /// <inheritdoc />
        public void Debug(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logDebug(
                this.logger,
                message,
                this.requestContext.InternalRequestId,
                this.requestContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logError(
                this.logger,
                message,
                this.requestContext.InternalRequestId,
                this.requestContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Info(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logInfo(
                this.logger,
                message,
                this.requestContext.InternalRequestId,
                this.requestContext.ExternalRequestId,
                exception);
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception = null)
        {
            this.AssertContextSet();

            this.logWarning(
                this.logger,
                message,
                this.requestContext.InternalRequestId,
                this.requestContext.ExternalRequestId,
                exception);
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303",
            Justification = "Library will not be localised - a resources file for this is overkill.")]
        private void AssertContextSet()
        {
            if (this.requestContext == null)
            {
                throw new InvalidOperationException(
                    $"No context for the logger set. You must call either " +
                    $"{nameof(this.SetContext)} or " +
                    $"{nameof(this.SetInternalRequestId)} prior to calling " +
                    $"any logging methods.");
            }
        }
    }
}